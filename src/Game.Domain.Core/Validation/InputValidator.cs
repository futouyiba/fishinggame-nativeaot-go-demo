using Game.Domain.Core.Model;

namespace Game.Domain.Core.Validation;

public static class InputValidator
{
    public static ValidationErrorDetail Validate(in StepInput input)
    {
        if (!IsFinite(input.Dt) ||
            !IsFinite(input.LureDepth) ||
            !IsFinite(input.LureSpeed) ||
            !IsFinite(input.Tension) ||
            !IsFinite(input.WaterTemp))
        {
            return ValidationErrorDetail.NonFiniteValue;
        }

        if (input.Dt <= 0f || input.Dt > 1f)
        {
            return ValidationErrorDetail.DtOutOfRange;
        }

        if (input.LureDepth < 0f || input.LureDepth > 60f)
        {
            return ValidationErrorDetail.LureDepthOutOfRange;
        }

        if (input.LureSpeed < 0f || input.LureSpeed > 25f)
        {
            return ValidationErrorDetail.LureSpeedOutOfRange;
        }

        if (input.Tension < 0f || input.Tension > 100f)
        {
            return ValidationErrorDetail.TensionOutOfRange;
        }

        if (input.WaterTemp < -5f || input.WaterTemp > 40f)
        {
            return ValidationErrorDetail.WaterTempOutOfRange;
        }

        return ValidationErrorDetail.None;
    }

    private static bool IsFinite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);
}
