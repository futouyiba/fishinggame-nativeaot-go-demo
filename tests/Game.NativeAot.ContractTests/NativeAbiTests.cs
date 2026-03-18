using System.Runtime.InteropServices;
using Game.NativeAot.Export.NativeTypes;

namespace Game.NativeAot.ContractTests;

public sealed class NativeAbiTests
{
    [Fact]
    public void StepInputNative_ShouldMatchExpectedAbi()
    {
        Assert.Equal(48, Marshal.SizeOf<StepInputNative>());
        Assert.Equal(0, OffsetOf<StepInputNative>(nameof(StepInputNative.CastId)));
        Assert.Equal(8, OffsetOf<StepInputNative>(nameof(StepInputNative.SeedBase)));
        Assert.Equal(16, OffsetOf<StepInputNative>(nameof(StepInputNative.PoseIndex)));
        Assert.Equal(20, OffsetOf<StepInputNative>(nameof(StepInputNative.SampleSlot)));
        Assert.Equal(24, OffsetOf<StepInputNative>(nameof(StepInputNative.Dt)));
        Assert.Equal(28, OffsetOf<StepInputNative>(nameof(StepInputNative.LureDepth)));
        Assert.Equal(32, OffsetOf<StepInputNative>(nameof(StepInputNative.LureSpeed)));
        Assert.Equal(36, OffsetOf<StepInputNative>(nameof(StepInputNative.Tension)));
        Assert.Equal(40, OffsetOf<StepInputNative>(nameof(StepInputNative.WaterTemp)));
        Assert.Equal(44, OffsetOf<StepInputNative>(nameof(StepInputNative.ActionFlags)));
    }

    [Fact]
    public void StepOutputNative_ShouldMatchExpectedAbi()
    {
        Assert.Equal(24, Marshal.SizeOf<StepOutputNative>());
        Assert.Equal(0, OffsetOf<StepOutputNative>(nameof(StepOutputNative.ResultCode)));
        Assert.Equal(4, OffsetOf<StepOutputNative>(nameof(StepOutputNative.BiteProb)));
        Assert.Equal(8, OffsetOf<StepOutputNative>(nameof(StepOutputNative.WeightFactor)));
        Assert.Equal(12, OffsetOf<StepOutputNative>(nameof(StepOutputNative.TensionDelta)));
        Assert.Equal(16, OffsetOf<StepOutputNative>(nameof(StepOutputNative.FishTypeId)));
        Assert.Equal(20, OffsetOf<StepOutputNative>(nameof(StepOutputNative.IsHit)));
    }

    [Fact]
    public void ErrorInfoNative_ShouldMatchExpectedAbi()
    {
        Assert.Equal(8, Marshal.SizeOf<ErrorInfoNative>());
        Assert.Equal(0, OffsetOf<ErrorInfoNative>(nameof(ErrorInfoNative.Code)));
        Assert.Equal(4, OffsetOf<ErrorInfoNative>(nameof(ErrorInfoNative.Detail)));
    }

    [Fact]
    public void SessionConfigNative_ShouldMatchExpectedAbi()
    {
        Assert.Equal(32, Marshal.SizeOf<SessionConfigNative>());
        Assert.Equal(0, OffsetOf<SessionConfigNative>(nameof(SessionConfigNative.CastId)));
        Assert.Equal(8, OffsetOf<SessionConfigNative>(nameof(SessionConfigNative.SeedBase)));
        Assert.Equal(16, OffsetOf<SessionConfigNative>(nameof(SessionConfigNative.PoseIndex)));
        Assert.Equal(20, OffsetOf<SessionConfigNative>(nameof(SessionConfigNative.InitialSampleSlot)));
        Assert.Equal(24, OffsetOf<SessionConfigNative>(nameof(SessionConfigNative.InitialTension)));
        Assert.Equal(28, OffsetOf<SessionConfigNative>(nameof(SessionConfigNative.WaterTemp)));
    }

    [Fact]
    public void SessionStepInputNative_ShouldMatchExpectedAbi()
    {
        Assert.Equal(16, Marshal.SizeOf<SessionStepInputNative>());
        Assert.Equal(0, OffsetOf<SessionStepInputNative>(nameof(SessionStepInputNative.Dt)));
        Assert.Equal(4, OffsetOf<SessionStepInputNative>(nameof(SessionStepInputNative.LureDepth)));
        Assert.Equal(8, OffsetOf<SessionStepInputNative>(nameof(SessionStepInputNative.LureSpeed)));
        Assert.Equal(12, OffsetOf<SessionStepInputNative>(nameof(SessionStepInputNative.ActionFlags)));
    }

    private static int OffsetOf<T>(string fieldName) where T : struct
    {
        return (int)Marshal.OffsetOf<T>(fieldName);
    }
}
