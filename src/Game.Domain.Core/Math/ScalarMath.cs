namespace Game.Domain.Core.Math;

public static class ScalarMath
{
    public static float Clamp(float value, float min, float max)
    {
        if (value < min)
        {
            return min;
        }

        if (value > max)
        {
            return max;
        }

        return value;
    }

    public static float Clamp01(float value) => Clamp(value, 0f, 1f);

    public static float InverseLerp(float min, float max, float value)
    {
        if (min == max)
        {
            return 0f;
        }

        return Clamp01((value - min) / (max - min));
    }

    public static float RemapClamped(float fromMin, float fromMax, float toMin, float toMax, float value)
    {
        var t = InverseLerp(fromMin, fromMax, value);
        return toMin + ((toMax - toMin) * t);
    }
}
