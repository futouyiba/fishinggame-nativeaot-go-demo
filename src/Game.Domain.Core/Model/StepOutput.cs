namespace Game.Domain.Core.Model;

public readonly record struct StepOutput(
    ResultCode ResultCode,
    float BiteProb,
    float WeightFactor,
    float TensionDelta,
    uint FishTypeId,
    bool IsHit)
{
    public static StepOutput Empty => new(ResultCode.ValidationFailed, 0f, 0f, 0f, 0u, false);
}
