using AutoMapper;
using AstronautHealthMonitoring.DTOs.SymptomGuidance;
using AstronautHealthMonitoring.Models.Entities;

namespace AstronautHealthMonitoring.Mapping;

public class SymptomGuidanceMappingProfile : Profile
{
    public SymptomGuidanceMappingProfile()
    {
        CreateMap<Symptom, SymptomDto>();

        CreateMap<ConditionSymptom, ConditionSymptomDto>()
            .ForMember(d => d.SymptomCode, opt => opt.MapFrom(s => s.Symptom != null ? s.Symptom.Code : string.Empty))
            .ForMember(d => d.SymptomName, opt => opt.MapFrom(s => s.Symptom != null ? s.Symptom.Name : string.Empty))
            .ForMember(d => d.IsRedFlag, opt => opt.MapFrom(s => s.Symptom != null && s.Symptom.IsRedFlag));

        CreateMap<ConditionVitalSign, ConditionVitalSignDto>()
            .ForMember(d => d.VitalSignTypeCode, opt => opt.MapFrom(s => s.VitalSignType != null ? s.VitalSignType.Code : string.Empty))
            .ForMember(d => d.VitalSignTypeName, opt => opt.MapFrom(s => s.VitalSignType != null ? s.VitalSignType.Name : string.Empty));

        CreateMap<SpaceCondition, SpaceConditionDto>()
            .ForMember(d => d.AssociatedSymptoms, opt => opt.MapFrom(s => s.ConditionSymptoms))
            .ForMember(d => d.AssociatedVitalSigns, opt => opt.MapFrom(s => s.ConditionVitalSigns));
    }
}
