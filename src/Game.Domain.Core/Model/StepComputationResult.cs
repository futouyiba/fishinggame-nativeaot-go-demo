namespace Game.Domain.Core.Model;

public readonly record struct StepComputationResult(
    ResultCode ResultCode,
    ValidationErrorDetail ValidationError,
    StepOutput Output)
{
    public bool IsSuccess => ResultCode == ResultCode.Ok;
}
