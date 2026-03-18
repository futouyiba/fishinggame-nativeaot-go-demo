using Game.Domain.Core.Math;
using Game.Domain.Core.Model;

namespace Game.Domain.Core.Rules;

public static class BiteRule
{
    public static float Evaluate(in StepInput input, float noise)
    {
        var tempFactor = ScalarMath.RemapClamped(6f, 24f, 0.2f, 1f, input.WaterTemp);
        var depthFactor = 1f - System.MathF.Abs(input.LureDepth - 5.5f) / 7.5f;
        var speedFactor = 1f - System.MathF.Abs(input.LureSpeed - 2.8f) / 4.2f;
        var actionBonus = (input.ActionFlags & 1) != 0 ? 0.08f : 0f;
        var tensionPenalty = ScalarMath.RemapClamped(0f, 80f, 1f, 0.35f, input.Tension);
        var blended = 0.08f +
                      (0.32f * ScalarMath.Clamp01(tempFactor)) +
                      (0.22f * ScalarMath.Clamp01(depthFactor)) +
                      (0.18f * ScalarMath.Clamp01(speedFactor)) +
                      actionBonus;
        var noisy = blended * (0.82f + (0.36f * noise));
        return ScalarMath.Clamp01(noisy * tensionPenalty);
    }
}
