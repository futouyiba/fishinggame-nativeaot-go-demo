using Game.Domain.Core.Model;
using Game.Domain.Core.Simulation;

namespace Game.Domain.Core.Tests;

public sealed class RealtimeStepCalculatorTests
{
    [Fact]
    public void SameInput_ShouldProduceSameOutput()
    {
        var input = new StepInput(101, 2024, 2, 7, 0.016f, 4.5f, 2.4f, 18f, 17f, 1);

        var first = RealtimeStepCalculator.Step(in input);
        var second = RealtimeStepCalculator.Step(in input);

        Assert.Equal(first, second);
        Assert.True(first.IsSuccess);
    }

    [Theory]
    [MemberData(nameof(GetGoldenCases))]
    public void GoldenCases_ShouldMatchExpected(GoldenCase goldenCase)
    {
        var input = new StepInput(
            goldenCase.Input.CastId,
            goldenCase.Input.SeedBase,
            goldenCase.Input.PoseIndex,
            goldenCase.Input.SampleSlot,
            goldenCase.Input.Dt,
            goldenCase.Input.LureDepth,
            goldenCase.Input.LureSpeed,
            goldenCase.Input.Tension,
            goldenCase.Input.WaterTemp,
            goldenCase.Input.ActionFlags);

        var result = RealtimeStepCalculator.Step(in input);

        Assert.True(result.IsSuccess);
        Assert.Equal(goldenCase.Expected.ResultCode, (int)result.Output.ResultCode);
        Assert.Equal(goldenCase.Expected.FishTypeId, result.Output.FishTypeId);
        Assert.Equal(goldenCase.Expected.IsHit, result.Output.IsHit);
        AssertClose(goldenCase.Expected.BiteProb, result.Output.BiteProb);
        AssertClose(goldenCase.Expected.WeightFactor, result.Output.WeightFactor);
        AssertClose(goldenCase.Expected.TensionDelta, result.Output.TensionDelta);
    }

    [Fact]
    public void FishingSession_ShouldAdvanceSampleSlotAndTension()
    {
        var session = new FishingSession(new SessionCreateInput(
            10001,
            20260318,
            3,
            12,
            18f,
            19f));

        var first = session.Step(new SessionStepInput(0.016f, 5.2f, 2.7f, 1));
        var second = session.Step(new SessionStepInput(0.016f, 5.2f, 2.7f, 1));

        Assert.True(first.IsSuccess);
        Assert.True(second.IsSuccess);
        AssertClose(0.5717213f, first.Output.BiteProb);
        AssertClose(10.740984f, first.Output.TensionDelta);
        Assert.NotEqual(first.Output, second.Output);

        var snapshot = session.Snapshot;
        Assert.Equal(14, snapshot.CurrentSampleSlot);
        Assert.InRange(snapshot.CurrentTension, 0f, 100f);
    }

    public static IEnumerable<object[]> GetGoldenCases()
    {
        return GoldenCaseLoader.Load().Select(testCase => new object[] { testCase });
    }

    private static void AssertClose(float expected, float actual)
    {
        Assert.InRange(actual, expected - 0.0001f, expected + 0.0001f);
    }
}
