using Game.Domain.Core.Model;

namespace Game.Domain.Core.Validation;

public static class SessionValidator
{
    public static ValidationErrorDetail Validate(in SessionCreateInput input)
    {
        if (!IsFinite(input.InitialTension) || !IsFinite(input.WaterTemp))
        {
            return ValidationErrorDetail.NonFiniteValue;
        }

        if (input.InitialTension < 0f || input.InitialTension > 100f)
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
