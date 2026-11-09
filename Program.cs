using System.Text;
using System.Threading.RateLimiting;
using AstronautHealthMonitoring.Data;
using AstronautHealthMonitoring.DTOs.Auth;
using AstronautHealthMonitoring.Models.Entities;
using AstronautHealthMonitoring.Services.Implementations;
using AstronautHealthMonitoring.Services.Interfaces;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Register Mapster configuration scanning this assembly and scoped IMapper
var typeAdapterConfig = new TypeAdapterConfig();
typeAdapterConfig.Scan(typeof(Program).Assembly);
builder.Services.AddSingleton(typeAdapterConfig);
builder.Services.AddScoped<IMapper, ServiceMapper>();

// Register FluentValidation validators from this assembly
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// Register stateless vital sign status evaluation engine
builder.Services.AddSingleton<IVitalSignStatusEvaluator, VitalSignStatusEvaluator>();

// Register AppDbContext with PostgreSQL (Supabase)
// Connection string is read from "ConnectionStrings:DefaultConnection" via user-secrets or appsettings
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=localhost;Database=astronaut_health_db;Username=postgres;Password=postgres";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 1. Configure ASP.NET Core Identity
var passwordOptions = builder.Configuration.GetSection("Identity:Password");
var lockoutOptions = builder.Configuration.GetSection("Identity:Lockout");

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password policies from configuration
    options.Password.RequireDigit = passwordOptions.GetValue("RequireDigit", true);
    options.Password.RequireLowercase = passwordOptions.GetValue("RequireLowercase", true);
    options.Password.RequireUppercase = passwordOptions.GetValue("RequireUppercase", true);
    options.Password.RequireNonAlphanumeric = passwordOptions.GetValue("RequireNonAlphanumeric", true);
    options.Password.RequiredLength = passwordOptions.GetValue("RequiredLength", 8);

    // Lockout policies from configuration
    options.Lockout.MaxFailedAccessAttempts = lockoutOptions.GetValue("MaxFailedAccessAttempts", 5);
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(lockoutOptions.GetValue("DefaultLockoutMinutes", 15));
    options.Lockout.AllowedForNewUsers = lockoutOptions.GetValue("AllowedForNewUsers", true);

    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// 2. Configure JWT Authentication & SignalR Query-String Handler
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
{
    if (builder.Environment.IsDevelopment())
    {
        jwtKey = "DevelopmentPlaceholder_SetViaUserSecrets_Min32BytesLong!";
    }
    else
    {
        throw new InvalidOperationException("JWT signing key is not configured. Set 'Jwt:Key' via environment variables or secrets.");
    }
}
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "AstronautHealthMonitoring";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "AstronautHealthMonitoringClient";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.FromSeconds(30)
    };

    // Authenticate SignalR connections where the token is delivered via query string under /hubs
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

// 3. Configure Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthPolicies.AstronautOnly, policy => policy.RequireRole(UserRoles.Astronaut));
    options.AddPolicy(AuthPolicies.MonitorOnly, policy => policy.RequireRole(UserRoles.Monitor));
    options.AddPolicy(AuthPolicies.Authenticated, policy => policy.RequireAuthenticatedUser());
});

// 4. Configure CORS from Configuration
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? new[] { "http://localhost:3000", "http://localhost:5173" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// 5. Configure Rate Limiter for Login Endpoint from Configuration
var rateLimitSection = builder.Configuration.GetSection("RateLimiting:Login");
var permitLimit = rateLimitSection.GetValue("PermitLimit", 5);
var windowMinutes = rateLimitSection.GetValue("WindowMinutes", 1.0);
var queueLimit = rateLimitSection.GetValue("QueueLimit", 0);

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter("LoginLimiter", opt =>
    {
        opt.PermitLimit = permitLimit;
        opt.Window = TimeSpan.FromMinutes(windowMinutes);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = queueLimit;
    });
});

// 6. Register Identity, Auth, and Context Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IIdentityDataSeeder, IdentityDataSeeder>();

var app = builder.Build();

// 7. Startup Check: Log which configuration provider supplied the database connection string (host only)
if (app.Configuration is IConfigurationRoot configRoot)
{
    string? winningProvider = null;
    string? rawConnectionString = null;

    foreach (var provider in configRoot.Providers.Reverse())
    {
        if (provider.TryGet("ConnectionStrings:DefaultConnection", out var val) && !string.IsNullOrWhiteSpace(val))
        {
            winningProvider = provider.ToString();
            rawConnectionString = val;
            break;
        }
    }

    if (!string.IsNullOrWhiteSpace(rawConnectionString))
    {
        var host = "Unknown";
        try
        {
            var npgsqlBuilder = new Npgsql.NpgsqlConnectionStringBuilder(rawConnectionString);
            host = npgsqlBuilder.Host;
        }
        catch
        {
            var match = System.Text.RegularExpressions.Regex.Match(rawConnectionString, @"Host=([^;]+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (match.Success) host = match.Groups[1].Value;
        }

        app.Logger.LogInformation("Database connection configured. Host: '{Host}', Resolved from configuration provider: '{Provider}'",
            host, winningProvider ?? "Unknown");
    }
    else
    {
        app.Logger.LogWarning("No connection string found for 'ConnectionStrings:DefaultConnection'.");
    }
}

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("DefaultCorsPolicy");
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 8. Run Identity Demo User Seeding (Development environment only or when SeedData:Enabled is true)
var seedingEnabled = app.Environment.IsDevelopment() || app.Configuration.GetValue("SeedData:Enabled", false);
if (seedingEnabled)
{
    try
    {
        using var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IIdentityDataSeeder>();
        await seeder.SeedAsync();
    }
    catch (InvalidOperationException)
    {
        // Re-throw fail-fast configuration error so startup halts immediately listing missing key names
        throw;
    }
    catch (Exception ex)
    {
        app.Logger.LogWarning("Skipping identity demo user seeding at startup (database may be pending migrations or unreachable): {Message}", ex.Message);
    }
}

app.Run();
