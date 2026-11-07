using AutoMapper;
using AstronautHealthMonitoring.DTOs.ReferenceRanges;
using AstronautHealthMonitoring.Models.Entities;

namespace AstronautHealthMonitoring.Mapping;

public class ReferenceRangeMappingProfile : Profile
{
    public ReferenceRangeMappingProfile()
    {
        CreateMap<ReferenceRange, ReferenceRangeDto>()
            .ForMember(d => d.VitalSignTypeCode, opt => opt.MapFrom(s => s.VitalSignType != null ? s.VitalSignType.Code : string.Empty));

        CreateMap<UpdateReferenceRangeRequest, ReferenceRange>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
            .ForMember(d => d.VitalSignTypeId, opt => opt.Ignore())
            .ForMember(d => d.VitalSignType, opt => opt.Ignore())
            .ForMember(d => d.ApplicableGender, opt => opt.Ignore())
            .ForMember(d => d.MinAge, opt => opt.Ignore())
            .ForMember(d => d.MaxAge, opt => opt.Ignore())
            .ForMember(d => d.ReviewedBy, opt => opt.Ignore())
            .ForMember(d => d.ReviewedAt, opt => opt.Ignore());
    }
}
