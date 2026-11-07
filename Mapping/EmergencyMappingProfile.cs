using AutoMapper;
using AstronautHealthMonitoring.DTOs.Emergency;
using AstronautHealthMonitoring.Models.Entities;

namespace AstronautHealthMonitoring.Mapping;

public class EmergencyMappingProfile : Profile
{
    public EmergencyMappingProfile()
    {
        CreateMap<EmergencyProcedure, EmergencyProcedureDto>();
    }
}

