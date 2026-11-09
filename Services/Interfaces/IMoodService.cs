using AstronautHealthMonitoring.DTOs.Mood;

namespace AstronautHealthMonitoring.Services.Interfaces;

/// <summary>
/// Service contract for recording and reviewing astronaut psychological check-ins and stress levels.
/// </summary>
public interface IMoodService
{
    /// <summary>
    /// Submits a psychological check-in entry for an astronaut; idempotent on ClientGeneratedId.
    /// </summary>
    Task<MoodEntryDto> RecordMoodAsync(int astronautId, CreateMoodEntryRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves psychological check-in history for an astronaut within a time range.
    /// </summary>
    Task<IReadOnlyList<MoodEntryDto>> GetMoodHistoryAsync(int astronautId, DateTime fromUtc, DateTime toUtc, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the latest psychological check-in entry for an astronaut.
    /// </summary>
    Task<MoodEntryDto?> GetLatestMoodEntryAsync(int astronautId, CancellationToken cancellationToken = default);
}

