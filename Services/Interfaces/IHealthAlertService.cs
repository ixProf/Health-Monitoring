using AstronautHealthMonitoring.DTOs.Alerts;

namespace AstronautHealthMonitoring.Services.Interfaces;

/// <summary>
/// Service contract for querying, acknowledging, and managing health safety alerts.
/// </summary>
public interface IHealthAlertService
{
    /// <summary>
    /// Retrieves all active safety alerts across all crew members for flight monitor consoles.
    /// </summary>
    Task<IReadOnlyList<HealthAlertDto>> GetActiveAlertsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves safety alerts for a specific astronaut, optionally filtered by active status.
    /// </summary>
    Task<IReadOnlyList<HealthAlertDto>> GetAlertsForAstronautAsync(int astronautId, bool activeOnly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single health alert by its unique database identifier.
    /// </summary>
    Task<HealthAlertDto?> GetAlertByIdAsync(int alertId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Acknowledges an active health alert and logs the monitor action taken.
    /// </summary>
    Task<HealthAlertDto> AcknowledgeAlertAsync(int alertId, AcknowledgeAlertRequest request, string monitorUserId, CancellationToken cancellationToken = default);
}

