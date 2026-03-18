namespace Game.Domain.Core.Model;

public readonly record struct SessionStepInput(
    float Dt,
    float LureDepth,
    float LureSpeed,
    int ActionFlags);
