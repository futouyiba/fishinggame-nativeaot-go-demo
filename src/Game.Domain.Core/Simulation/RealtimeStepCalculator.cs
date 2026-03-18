using Game.Domain.Core.Model;
using Game.Domain.Core.Rng;
using Game.Domain.Core.Rules;
using Game.Domain.Core.Validation;

namespace Game.Domain.Core.Simulation;

public static class RealtimeStepCalculator
{
    public static StepComputationResult Step(in StepInput input)
    {
        var validation = InputValidator.Validate(in input);
        if (validation != ValidationErrorDetail.None)
        {
            return new StepComputationResult(
                ResultCode.ValidationFailed,
                validation,
                StepOutput.Empty);
        }

        var rng = new SplitMix64(SeedComposer.Compose(in input));
        var biteNoise = rng.NextFloat01();
        var weightNoise = rng.NextFloat01();
        var hitNoise = rng.NextFloat01();

        var biteProb = BiteRule.Evaluate(in input, biteNoise);
        var weightFactor = WeightRule.Evaluate(in input, biteProb, weightNoise);
        var isHit = biteProb >= 0.42f && (hitNoise + (biteProb * 0.35f)) >= 0.62f;
        var tensionDelta = TensionRule.Evaluate(in input, biteProb, isHit);
        var fishTypeId = FishRule.ResolveFishType(weightFactor, hitNoise);

        var output = new StepOutput(
            ResultCode.Ok,
            biteProb,
            weightFactor,
            tensionDelta,
            fishTypeId,
            isHit);

        return new StepComputationResult(ResultCode.Ok, ValidationErrorDetail.None, output);
    }
}
