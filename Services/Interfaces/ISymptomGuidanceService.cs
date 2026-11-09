using AstronautHealthMonitoring.DTOs.SymptomGuidance;

namespace AstronautHealthMonitoring.Services.Interfaces;

/// <summary>
/// Service contract for symptom catalogs, space condition guidance, and non-diagnostic topic evaluation.
/// </summary>
public interface ISymptomGuidanceService
{
    /// <summary>
    /// Retrieves the full catalog of observable signs and subjective symptoms.
    /// </summary>
    Task<IReadOnlyList<SymptomDto>> GetAllSymptomsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all spaceflight adaptation conditions and non-diagnostic guidance topics.
    /// </summary>
    Task<IReadOnlyList<SpaceConditionDto>> GetAllConditionsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Evaluates reported symptom codes and returns ranked possible related health topics using non-diagnostic associations.
    /// </summary>
    Task<IReadOnlyList<ConditionMatchResultDto>> EvaluateSymptomsAsync(EvaluateSymptomsRequest request, CancellationToken cancellationToken = default);
}

