using FluentValidation;
using AstronautHealthMonitoring.DTOs.Alerts;

namespace AstronautHealthMonitoring.Validators.Alerts;

public class AcknowledgeAlertRequestValidator : AbstractValidator<AcknowledgeAlertRequest>
{
    public AcknowledgeAlertRequestValidator()
    {
        RuleFor(x => x.ActionTaken)
            .MaximumLength(1000);
    }
}

