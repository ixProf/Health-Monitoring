using AutoMapper;
using AstronautHealthMonitoring.DTOs.Astronaut;
using AstronautHealthMonitoring.Models.Entities;

namespace AstronautHealthMonitoring.Mapping;

public class AstronautMappingProfile : Profile
{
    public AstronautMappingProfile()
    {
        CreateMap<Astronaut, AstronautDto>()
            .ForMember(d => d.MissionName, opt => opt.MapFrom(s => s.Mission != null ? s.Mission.Name : null));

        CreateMap<CreateAstronautRequest, Astronaut>()
            .ForMember(d => d.FullName, opt => opt.MapFrom(s => $"{s.FirstName} {s.LastName}".Trim()))
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
            .ForMember(d => d.Mission, opt => opt.Ignore())
            .ForMember(d => d.VitalSignReadings, opt => opt.Ignore())
            .ForMember(d => d.SleepRecords, opt => opt.Ignore())
            .ForMember(d => d.MoodEntries, opt => opt.Ignore())
            .ForMember(d => d.Recommendations, opt => opt.Ignore())
            .ForMember(d => d.HealthAlerts, opt => opt.Ignore())
            .ForMember(d => d.ChatMessages, opt => opt.Ignore());

        CreateMap<UpdateAstronautRequest, Astronaut>()
            .ForMember(d => d.FullName, opt => opt.MapFrom(s => $"{s.FirstName} {s.LastName}".Trim()))
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.UserId, opt => opt.Ignore())
            .ForMember(d => d.AstronautCode, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
            .ForMember(d => d.Mission, opt => opt.Ignore())
            .ForMember(d => d.VitalSignReadings, opt => opt.Ignore())
            .ForMember(d => d.SleepRecords, opt => opt.Ignore())
            .ForMember(d => d.MoodEntries, opt => opt.Ignore())
            .ForMember(d => d.Recommendations, opt => opt.Ignore())
            .ForMember(d => d.HealthAlerts, opt => opt.Ignore())
            .ForMember(d => d.ChatMessages, opt => opt.Ignore());
    }
}
