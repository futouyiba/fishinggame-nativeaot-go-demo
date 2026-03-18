using System.Text.Json.Serialization;

namespace Game.Domain.Core.Tests;

public sealed class GoldenCaseFile
{
    [JsonPropertyName("cases")]
    public List<GoldenCase> Cases { get; set; } = [];
}

public sealed class GoldenCase
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("input")]
    public GoldenInput Input { get; set; } = new();

    [JsonPropertyName("expected")]
    public GoldenExpected Expected { get; set; } = new();
}

public sealed class GoldenInput
{
    [JsonPropertyName("castId")]
    public ulong CastId { get; set; }

    [JsonPropertyName("seedBase")]
    public ulong SeedBase { get; set; }

    [JsonPropertyName("poseIndex")]
    public int PoseIndex { get; set; }

    [JsonPropertyName("sampleSlot")]
    public int SampleSlot { get; set; }

    [JsonPropertyName("dt")]
    public float Dt { get; set; }

    [JsonPropertyName("lureDepth")]
    public float LureDepth { get; set; }

    [JsonPropertyName("lureSpeed")]
    public float LureSpeed { get; set; }

    [JsonPropertyName("tension")]
    public float Tension { get; set; }

    [JsonPropertyName("waterTemp")]
    public float WaterTemp { get; set; }

    [JsonPropertyName("actionFlags")]
    public int ActionFlags { get; set; }
}

public sealed class GoldenExpected
{
    [JsonPropertyName("resultCode")]
    public int ResultCode { get; set; }

    [JsonPropertyName("biteProb")]
    public float BiteProb { get; set; }

    [JsonPropertyName("weightFactor")]
    public float WeightFactor { get; set; }

    [JsonPropertyName("tensionDelta")]
    public float TensionDelta { get; set; }

    [JsonPropertyName("fishTypeId")]
    public uint FishTypeId { get; set; }

    [JsonPropertyName("isHit")]
    public bool IsHit { get; set; }
}
