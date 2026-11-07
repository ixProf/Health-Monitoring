using AutoMapper;
using AstronautHealthMonitoring.Mapping;
using Xunit;

namespace AstronautHealthMonitoring.Tests;

public class AutoMapperConfigurationTests
{
    [Fact]
    public void AutoMapper_Configuration_IsValid()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(AstronautMappingProfile).Assembly);
        });

        config.AssertConfigurationIsValid();
    }
}

