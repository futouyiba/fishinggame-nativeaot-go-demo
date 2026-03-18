namespace Game.Domain.Core.Model;

public readonly record struct SessionSnapshot(
    int CurrentSampleSlot,
    float CurrentTension,
    float WaterTemp);
