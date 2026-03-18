using System.Text.Json;
using System.Text.Json.Nodes;
using Game.Domain.Core.Model;
using Game.Domain.Core.Simulation;

var repoRoot = ResolveRepoRoot();
var goldenPath = Path.Combine(repoRoot, "tests", "GoldenCases", "step_cases.json");
var json = JsonNode.Parse(File.ReadAllText(goldenPath))!.AsObject();
var cases = json["cases"]!.AsArray();
var serializerOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true
};

foreach (var item in cases)
{
    var testCase = item!.AsObject();
    var input = testCase["input"]!.Deserialize<GoldenInput>(serializerOptions)!;

    var result = RealtimeStepCalculator.Step(new StepInput(
        input.CastId,
        input.SeedBase,
        input.PoseIndex,
        input.SampleSlot,
        input.Dt,
        input.LureDepth,
        input.LureSpeed,
        input.Tension,
        input.WaterTemp,
        input.ActionFlags));

    testCase["expected"] = JsonSerializer.SerializeToNode(new GoldenExpected(
        (int)result.Output.ResultCode,
        result.Output.BiteProb,
        result.Output.WeightFactor,
        result.Output.TensionDelta,
        result.Output.FishTypeId,
        result.Output.IsHit), serializerOptions);
}

File.WriteAllText(goldenPath, json.ToJsonString(serializerOptions));
Console.WriteLine($"Golden cases updated: {goldenPath}");

static string ResolveRepoRoot()
{
    var current = new DirectoryInfo(AppContext.BaseDirectory);
    while (current is not null)
    {
        var candidate = Path.Combine(current.FullName, "tests", "GoldenCases", "step_cases.json");
        if (File.Exists(candidate))
        {
            return current.FullName;
        }

        current = current.Parent;
    }

    throw new DirectoryNotFoundException("Could not locate tests/GoldenCases/step_cases.json");
}

internal sealed record GoldenInput(
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

internal sealed record GoldenExpected(
    int ResultCode,
    float BiteProb,
    float WeightFactor,
    float TensionDelta,
    uint FishTypeId,
    bool IsHit);
