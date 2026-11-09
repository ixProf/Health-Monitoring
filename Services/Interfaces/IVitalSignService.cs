using AstronautHealthMonitoring.DTOs.VitalSigns;

namespace AstronautHealthMonitoring.Services.Interfaces;

/// <summary>
/// Service contract for recording, batch syncing, and querying physiological telemetry.
/// </summary>
public interface IVitalSignService
{
    /// <summary>
    /// Records a vital sign reading for an astronaut; idempotent on ClientGeneratedId to ignore offline sync duplicates.
    /// </summary>
    Task<VitalSignReadingDto> RecordReadingAsync(int astronautId, CreateVitalSignReadingRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ingests a batch of offline vital sign readings for an astronaut; idempotent on ClientGeneratedId for each reading.
    /// </summary>
    Task<IReadOnlyList<VitalSignReadingDto>> SyncReadingsAsync(int astronautId, SyncVitalSignReadingsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the most recent vital sign readings for an astronaut.
    /// </summary>
    Task<IReadOnlyList<VitalSignReadingDto>> GetRecentReadingsAsync(int astronautId, int count = 50, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves vital sign readings for an astronaut within a specified device capture time window.
    /// </summary>
    Task<IReadOnlyList<VitalSignReadingDto>> GetReadingsByTimeRangeAsync(int astronautId, DateTime fromUtc, DateTime toUtc, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all active monitored vital sign types and metric units.
    /// </summary>
    Task<IReadOnlyList<VitalSignTypeDto>> GetAllVitalSignTypesAsync(CancellationToken cancellationToken = default);
}

