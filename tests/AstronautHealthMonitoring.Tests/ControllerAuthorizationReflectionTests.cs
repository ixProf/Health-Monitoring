using System.Reflection;
using AstronautHealthMonitoring.Controllers;
using AstronautHealthMonitoring.DTOs.Auth;
using AstronautHealthMonitoring.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AstronautHealthMonitoring.Tests;

public class ControllerAuthorizationReflectionTests
{
    [Theory]
    [InlineData(typeof(VitalSignsController), nameof(VitalSignsController.RecordReading), AuthPolicies.AstronautOnly)]
    [InlineData(typeof(VitalSignsController), nameof(VitalSignsController.GetById), AuthPolicies.Authenticated)]
    [InlineData(typeof(VitalSignsController), nameof(VitalSignsController.GetLatest), AuthPolicies.Authenticated)]
    [InlineData(typeof(VitalSignsController), nameof(VitalSignsController.GetHistory), AuthPolicies.Authenticated)]
    [InlineData(typeof(HealthAlertsController), nameof(HealthAlertsController.GetAlerts), AuthPolicies.Authenticated)]
    [InlineData(typeof(HealthAlertsController), nameof(HealthAlertsController.GetById), AuthPolicies.Authenticated)]
    [InlineData(typeof(HealthAlertsController), nameof(HealthAlertsController.TriggerManualAlert), AuthPolicies.Authenticated)]
    [InlineData(typeof(HealthAlertsController), nameof(HealthAlertsController.AcknowledgeAlert), AuthPolicies.MonitorOnly)]
    [InlineData(typeof(HealthAlertsController), nameof(HealthAlertsController.ResolveAlert), AuthPolicies.MonitorOnly)]
    [InlineData(typeof(SleepController), nameof(SleepController.RecordSleep), AuthPolicies.AstronautOnly)]
    [InlineData(typeof(SleepController), nameof(SleepController.GetById), AuthPolicies.Authenticated)]
    [InlineData(typeof(SleepController), nameof(SleepController.GetLatest), AuthPolicies.Authenticated)]
    [InlineData(typeof(SleepController), nameof(SleepController.GetHistory), AuthPolicies.Authenticated)]
    [InlineData(typeof(MoodController), nameof(MoodController.RecordMood), AuthPolicies.AstronautOnly)]
    [InlineData(typeof(MoodController), nameof(MoodController.GetById), AuthPolicies.Authenticated)]
    [InlineData(typeof(MoodController), nameof(MoodController.GetLatest), AuthPolicies.Authenticated)]
    [InlineData(typeof(MoodController), nameof(MoodController.GetHistory), AuthPolicies.Authenticated)]
    public void Endpoint_Has_Expected_AuthorizePolicy(Type controllerType, string methodName, string expectedPolicy)
    {
        var method = controllerType.GetMethod(methodName);
        Assert.NotNull(method);

        var authorizeAttr = method.GetCustomAttribute<AuthorizeAttribute>();
        Assert.NotNull(authorizeAttr);
        Assert.Equal(expectedPolicy, authorizeAttr.Policy);
    }

    [Fact]
    public void AlertsHub_Class_Has_AuthorizeAttribute()
    {
        var authorizeAttr = typeof(AlertsHub).GetCustomAttribute<AuthorizeAttribute>();
        Assert.NotNull(authorizeAttr);
    }

    [Theory]
    [InlineData(typeof(VitalSignsController))]
    [InlineData(typeof(HealthAlertsController))]
    [InlineData(typeof(SleepController))]
    [InlineData(typeof(MoodController))]
    public void Controllers_Have_ApiController_And_Route_Attributes(Type controllerType)
    {
        var apiControllerAttr = controllerType.GetCustomAttribute<ApiControllerAttribute>();
        Assert.NotNull(apiControllerAttr);

        var routeAttr = controllerType.GetCustomAttribute<RouteAttribute>();
        Assert.NotNull(routeAttr);
        Assert.Equal("api/[controller]", routeAttr.Template);
    }
}

