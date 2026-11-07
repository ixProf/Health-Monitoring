using AutoMapper;
using AstronautHealthMonitoring.DTOs.Chat;
using AstronautHealthMonitoring.Models.Entities;

namespace AstronautHealthMonitoring.Mapping;

public class ChatMappingProfile : Profile
{
    public ChatMappingProfile()
    {
        CreateMap<ChatMessage, ChatMessageDto>()
            .ForMember(d => d.AstronautFullName, opt => opt.MapFrom(s => s.Astronaut != null ? s.Astronaut.FullName : string.Empty));

        CreateMap<SendChatMessageRequest, ChatMessage>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
            .ForMember(d => d.Astronaut, opt => opt.Ignore())
            .ForMember(d => d.SenderUserId, opt => opt.Ignore())
            .ForMember(d => d.SenderRole, opt => opt.Ignore())
            .ForMember(d => d.SentAt, opt => opt.Ignore())
            .ForMember(d => d.ReadAt, opt => opt.Ignore());
    }
}
