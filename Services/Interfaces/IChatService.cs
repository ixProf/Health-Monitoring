using AstronautHealthMonitoring.DTOs.Chat;

namespace AstronautHealthMonitoring.Services.Interfaces;

/// <summary>
/// Service contract for astronaut-to-ground monitor communication threads.
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Posts a message to the astronaut support communication thread; idempotent on ClientGeneratedId.
    /// </summary>
    Task<ChatMessageDto> SendMessageAsync(int astronautId, SendChatMessageRequest request, string senderUserId, string senderRole, CancellationToken cancellationToken = default);

    /// <summary>
    /// Loads chronological communication history for an astronaut support thread.
    /// </summary>
    Task<IReadOnlyList<ChatMessageDto>> GetThreadHistoryAsync(int astronautId, int take = 50, DateTime? beforeUtc = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks unread messages in an astronaut support thread as read.
    /// </summary>
    Task<bool> MarkThreadAsReadAsync(int astronautId, string readerUserId, CancellationToken cancellationToken = default);
}

