using AutoMapper;
using AstronautHealthMonitoring.DTOs.Mood;
using AstronautHealthMonitoring.Models.Entities;

namespace AstronautHealthMonitoring.Mapping;

public class MoodMappingProfile : Profile
{
    public MoodMappingProfile()
    {
        CreateMap<MoodEntry, MoodEntryDto>()
            .ForMember(d => d.AstronautFullName, opt => opt.MapFrom(s => s.Astronaut != null ? s.Astronaut.FullName : string.Empty));

        CreateMap<CreateMoodEntryRequest, MoodEntry>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
            .ForMember(d => d.Astronaut, opt => opt.Ignore());
    }
}
