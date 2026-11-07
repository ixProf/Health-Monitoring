using AutoMapper;
using AstronautHealthMonitoring.DTOs.VitalSigns;
using AstronautHealthMonitoring.Models.Entities;

namespace AstronautHealthMonitoring.Mapping;

public class VitalSignMappingProfile : Profile
{
    public VitalSignMappingProfile()
    {
        CreateMap<VitalSignReading, VitalSignReadingDto>()
            .ForMember(d => d.AstronautFullName, opt => opt.MapFrom(s => s.Astronaut != null ? s.Astronaut.FullName : string.Empty))
            .ForMember(d => d.VitalSignTypeCode, opt => opt.MapFrom(s => s.VitalSignType != null ? s.VitalSignType.Code : string.Empty))
            .ForMember(d => d.VitalSignTypeName, opt => opt.MapFrom(s => s.VitalSignType != null ? s.VitalSignType.Name : string.Empty))
            .ForMember(d => d.Unit, opt => opt.MapFrom(s => s.VitalSignType != null ? s.VitalSignType.Unit : string.Empty));

        CreateMap<CreateVitalSignReadingRequest, VitalSignReading>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
            .ForMember(d => d.Astronaut, opt => opt.Ignore())
            .ForMember(d => d.VitalSignType, opt => opt.Ignore())
            .ForMember(d => d.Status, opt => opt.Ignore())
            .ForMember(d => d.HealthAlerts, opt => opt.Ignore());

        CreateMap<VitalSignType, VitalSignTypeDto>();
    }
}
