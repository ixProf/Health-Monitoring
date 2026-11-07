using AutoMapper;
using AstronautHealthMonitoring.DTOs.Missions;
using AstronautHealthMonitoring.Models.Entities;

namespace AstronautHealthMonitoring.Mapping;

public class MissionMappingProfile : Profile
{
    public MissionMappingProfile()
    {
        CreateMap<Mission, MissionDto>()
            .ForMember(d => d.AstronautsCount, opt => opt.MapFrom(s => s.Astronauts.Count));

        CreateMap<CreateMissionRequest, Mission>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
            .ForMember(d => d.Astronauts, opt => opt.Ignore());

        CreateMap<UpdateMissionRequest, Mission>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
            .ForMember(d => d.Code, opt => opt.Ignore())
            .ForMember(d => d.Astronauts, opt => opt.Ignore());
    }
}
