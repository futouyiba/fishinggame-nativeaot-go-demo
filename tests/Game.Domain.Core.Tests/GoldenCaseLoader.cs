using System.Text.Json;

namespace Game.Domain.Core.Tests;

internal static class GoldenCaseLoader
{
    public static IReadOnlyList<GoldenCase> Load()
    {
        var path = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "..",
            "..",
            "..",
            "..",
            "GoldenCases",
            "step_cases.json"));

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<GoldenCaseFile>(json)?.Cases
               ?? throw new InvalidOperationException("Unable to load golden cases.");
    }
}
