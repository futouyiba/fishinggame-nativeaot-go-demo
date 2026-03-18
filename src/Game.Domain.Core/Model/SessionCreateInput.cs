namespace Game.Domain.Core.Model;

public readonly record struct SessionCreateInput(
    ulong CastId,
    ulong SeedBase,
    int PoseIndex,
    int InitialSampleSlot,
    float InitialTension,
    float WaterTemp);
