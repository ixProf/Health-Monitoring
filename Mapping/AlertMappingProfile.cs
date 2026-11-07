using AutoMapper;
using AstronautHealthMonitoring.DTOs.Alerts;
using AstronautHealthMonitoring.Models.Entities;

namespace AstronautHealthMonitoring.Mapping;

public class AlertMappingProfile : Profile
{
    public AlertMappingProfile()
    {
        CreateMap<HealthAlert, HealthAlertDto>()
            .ForMember(d => d.AstronautFullName, opt => opt.MapFrom(s => s.Astronaut != null ? s.Astronaut.FullName : string.Empty))
            .ForMember(d => d.VitalSignTypeId, opt => opt.MapFrom(s => s.VitalSignReading != null ? (int?)s.VitalSignReading.VitalSignTypeId : null))
            .ForMember(d => d.VitalSignTypeCode, opt => opt.MapFrom(s => s.VitalSignReading != null && s.VitalSignReading.VitalSignType != null ? s.VitalSignReading.VitalSignType.Code : null))
            .ForMember(d => d.EmergencyProcedureCode, opt => opt.MapFrom(s => s.EmergencyProcedure != null ? s.EmergencyProcedure.Code : null))
            .ForMember(d => d.EmergencyProcedureTitle, opt => opt.MapFrom(s => s.EmergencyProcedure != null ? s.EmergencyProcedure.Title : null));
    }
}
