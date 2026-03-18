using Game.Domain.Core.Model;
using UnityEngine;

namespace NativeAotGoDemo.Unity.Runtime;

[System.Serializable]
public sealed class StepDebugSnapshot
{
    public int resultCode;
    public float biteProb;
    public float weightFactor;
    public float tensionDelta;
    public uint fishTypeId;
    public bool isHit;

    public static StepDebugSnapshot FromResult(StepComputationResult result)
    {
        return new StepDebugSnapshot
        {
            resultCode = (int)result.ResultCode,
            biteProb = result.Output.BiteProb,
            weightFactor = result.Output.WeightFactor,
            tensionDelta = result.Output.TensionDelta,
            fishTypeId = result.Output.FishTypeId,
            isHit = result.Output.IsHit
        };
    }
}
