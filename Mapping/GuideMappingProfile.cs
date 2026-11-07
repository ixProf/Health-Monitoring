using AutoMapper;
using AstronautHealthMonitoring.DTOs.Guides;
using AstronautHealthMonitoring.Models.Entities;

namespace AstronautHealthMonitoring.Mapping;

public class GuideMappingProfile : Profile
{
    public GuideMappingProfile()
    {
        CreateMap<HealthMetricGuide, HealthMetricGuideDto>()
            .ForMember(d => d.VitalSignTypeCode, opt => opt.MapFrom(s => s.VitalSignType != null ? s.VitalSignType.Code : string.Empty));

        CreateMap<UpdateHealthMetricGuideRequest, HealthMetricGuide>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
            .ForMember(d => d.VitalSignTypeId, opt => opt.Ignore())
            .ForMember(d => d.VitalSignType, opt => opt.Ignore());
    }
}
