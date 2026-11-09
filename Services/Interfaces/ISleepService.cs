using AstronautHealthMonitoring.DTOs.Sleep;

namespace AstronautHealthMonitoring.Services.Interfaces;

/// <summary>
/// Service contract for recording and reviewing circadian stability and sleep telemetry.
/// </summary>
public interface ISleepService
{
    /// <summary>
    /// Logs a sleep session for an astronaut; idempotent on ClientGeneratedId to prevent duplicate ingestion.
    /// </summary>
    Task<SleepRecordDto> RecordSleepAsync(int astronautId, CreateSleepRecordRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves sleep records for an astronaut within a specified date window.
    /// </summary>
    Task<IReadOnlyList<SleepRecordDto>> GetSleepHistoryAsync(int astronautId, DateTime fromUtc, DateTime toUtc, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the most recent sleep session recorded for an astronaut.
    /// </summary>
    Task<SleepRecordDto?> GetLatestSleepRecordAsync(int astronautId, CancellationToken cancellationToken = default);
}

