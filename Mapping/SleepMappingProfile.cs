using AutoMapper;
using AstronautHealthMonitoring.DTOs.Sleep;
using AstronautHealthMonitoring.Models.Entities;

namespace AstronautHealthMonitoring.Mapping;

public class SleepMappingProfile : Profile
{
    public SleepMappingProfile()
    {
        CreateMap<SleepRecord, SleepRecordDto>()
            .ForMember(d => d.AstronautFullName, opt => opt.MapFrom(s => s.Astronaut != null ? s.Astronaut.FullName : string.Empty));

        CreateMap<CreateSleepRecordRequest, SleepRecord>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
            .ForMember(d => d.Astronaut, opt => opt.Ignore())
            .ForMember(d => d.DurationHours, opt => opt.Ignore());
    }
}
