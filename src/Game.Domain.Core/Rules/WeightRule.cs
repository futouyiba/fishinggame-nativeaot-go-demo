using Game.Domain.Core.Math;
using Game.Domain.Core.Model;

namespace Game.Domain.Core.Rules;

public static class WeightRule
{
    public static float Evaluate(in StepInput input, float biteProb, float noise)
    {
        var depthFactor = ScalarMath.RemapClamped(0f, 18f, 0.75f, 1.65f, input.LureDepth);
        var tempFactor = ScalarMath.RemapClamped(4f, 24f, 0.85f, 1.25f, input.WaterTemp);
        var biteFactor = ScalarMath.RemapClamped(0f, 1f, 0.8f, 1.35f, biteProb);
        var noiseFactor = 0.9f + (noise * 0.45f);
        return depthFactor * tempFactor * biteFactor * noiseFactor;
    }
}
