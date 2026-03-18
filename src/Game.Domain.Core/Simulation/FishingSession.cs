using Game.Domain.Core.Math;
using Game.Domain.Core.Model;
using Game.Domain.Core.Validation;

namespace Game.Domain.Core.Simulation;

public sealed class FishingSession
{
    private readonly object gate = new();
    private readonly ulong castId;
    private readonly ulong seedBase;
    private readonly int poseIndex;
    private readonly float waterTemp;

    private int sampleSlot;
    private float tension;

    public FishingSession(in SessionCreateInput input)
    {
        var validation = SessionValidator.Validate(in input);
        if (validation != ValidationErrorDetail.None)
        {
            throw new ArgumentOutOfRangeException(nameof(input), validation, "Invalid session bootstrap input.");
        }

        castId = input.CastId;
        seedBase = input.SeedBase;
        poseIndex = input.PoseIndex;
        sampleSlot = input.InitialSampleSlot;
        tension = input.InitialTension;
        waterTemp = input.WaterTemp;
    }

    public StepComputationResult Step(in SessionStepInput input)
    {
        lock (gate)
        {
            var stepInput = new StepInput(
                castId,
                seedBase,
                poseIndex,
                sampleSlot,
                input.Dt,
                input.LureDepth,
                input.LureSpeed,
                tension,
                waterTemp,
                input.ActionFlags);

            var result = RealtimeStepCalculator.Step(in stepInput);
            if (result.IsSuccess)
            {
                sampleSlot++;
                tension = ScalarMath.Clamp(tension + result.Output.TensionDelta, 0f, 100f);
            }

            return result;
        }
    }

    public SessionSnapshot Snapshot
    {
        get
        {
            lock (gate)
            {
                return new SessionSnapshot(sampleSlot, tension, waterTemp);
            }
        }
    }
}
