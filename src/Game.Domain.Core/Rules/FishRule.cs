namespace Game.Domain.Core.Rules;

public static class FishRule
{
    public static uint ResolveFishType(float weightFactor, float noise)
    {
        var score = weightFactor + (noise * 0.75f);

        if (score < 1.5f)
        {
            return 1001;
        }

        if (score < 2.1f)
        {
            return 2001;
        }

        if (score < 2.8f)
        {
            return 3001;
        }

        return 4001;
    }
}
