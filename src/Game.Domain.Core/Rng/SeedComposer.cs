using Game.Domain.Core.Model;

namespace Game.Domain.Core.Rng;

public static class SeedComposer
{
    public static ulong Compose(in StepInput input)
    {
        var seed = input.SeedBase;
        seed = Mix(seed, input.CastId);
        seed = Mix(seed, unchecked((ulong)(uint)input.PoseIndex));
        seed = Mix(seed, unchecked((ulong)(uint)input.SampleSlot));
        seed = Mix(seed, FloatToBits(input.Dt));
        seed = Mix(seed, FloatToBits(input.LureDepth));
        seed = Mix(seed, FloatToBits(input.LureSpeed));
        seed = Mix(seed, FloatToBits(input.Tension));
        seed = Mix(seed, FloatToBits(input.WaterTemp));
        seed = Mix(seed, unchecked((ulong)(uint)input.ActionFlags));
        return seed;
    }

    private static ulong Mix(ulong seed, ulong value)
    {
        var mixed = seed ^ (value + 0x9E3779B97F4A7C15UL + (seed << 6) + (seed >> 2));
        mixed ^= mixed >> 30;
        mixed *= 0xBF58476D1CE4E5B9UL;
        mixed ^= mixed >> 27;
        mixed *= 0x94D049BB133111EBUL;
        return mixed ^ (mixed >> 31);
    }

    private static uint FloatToBits(float value)
    {
        return BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
    }
}
