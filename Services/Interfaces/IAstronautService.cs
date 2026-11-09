using AstronautHealthMonitoring.DTOs.Astronaut;

namespace AstronautHealthMonitoring.Services.Interfaces;

/// <summary>
/// Service contract for managing astronaut demographic and flight assignment profiles.
/// </summary>
public interface IAstronautService
{
    /// <summary>
    /// Retrieves all astronaut profiles for flight monitor oversight.
    /// </summary>
    Task<IReadOnlyList<AstronautDto>> GetAllAstronautsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an astronaut profile by its unique database identifier.
    /// </summary>
    Task<AstronautDto?> GetAstronautByIdAsync(int astronautId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an astronaut profile by operational astronaut code.
    /// </summary>
    Task<AstronautDto?> GetAstronautByCodeAsync(string astronautCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new astronaut profile.
    /// </summary>
    Task<AstronautDto> CreateAstronautAsync(CreateAstronautRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing astronaut profile.
    /// </summary>
    Task<AstronautDto> UpdateAstronautAsync(int astronautId, UpdateAstronautRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Assigns or reassigns an astronaut to an operational flight mission.
    /// </summary>
    Task<bool> AssignMissionAsync(int astronautId, int? missionId, CancellationToken cancellationToken = default);
}

