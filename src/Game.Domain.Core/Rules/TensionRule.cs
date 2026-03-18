using Game.Domain.Core.Math;
using Game.Domain.Core.Model;

namespace Game.Domain.Core.Rules;

public static class TensionRule
{
    public static float Evaluate(in StepInput input, float biteProb, bool isHit)
    {
        var baseDelta = (biteProb * 18f) + (input.LureSpeed * 0.9f) - (input.Tension * 0.11f);
        if (isHit)
        {
            baseDelta += 3.5f;
        }

        return ScalarMath.Clamp(baseDelta, -12f, 12f);
    }
}
