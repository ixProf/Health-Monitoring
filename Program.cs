using AstronautHealthMonitoring.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Register AutoMapper profiles from this assembly
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Register FluentValidation validators from this assembly
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// Register AppDbContext with PostgreSQL (Supabase)
// Connection string is read from "ConnectionStrings:DefaultConnection" via user-secrets or appsettings
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=localhost;Database=astronaut_health_db;Username=postgres;Password=postgres";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
