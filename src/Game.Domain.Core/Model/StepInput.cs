namespace Game.Domain.Core.Model;

public readonly record struct StepInput(
    ulong CastId,
    ulong SeedBase,
    int PoseIndex,
    int SampleSlot,
    float Dt,
    float LureDepth,
    float LureSpeed,
    float Tension,
    float WaterTemp,
    int ActionFlags);
