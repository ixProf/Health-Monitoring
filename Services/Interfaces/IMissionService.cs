using AstronautHealthMonitoring.DTOs.Missions;

namespace AstronautHealthMonitoring.Services.Interfaces;

/// <summary>
/// Service contract for managing operational flight exploration missions.
/// </summary>
public interface IMissionService
{
    /// <summary>
    /// Retrieves all operational flight missions.
    /// </summary>
    Task<IReadOnlyList<MissionDto>> GetAllMissionsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a mission by its unique database identifier.
    /// </summary>
    Task<MissionDto?> GetMissionByIdAsync(int missionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a mission by its operational code.
    /// </summary>
    Task<MissionDto?> GetMissionByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new operational flight mission.
    /// </summary>
    Task<MissionDto> CreateMissionAsync(CreateMissionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an operational flight mission.
    /// </summary>
    Task<MissionDto> UpdateMissionAsync(int missionId, UpdateMissionRequest request, CancellationToken cancellationToken = default);
}

