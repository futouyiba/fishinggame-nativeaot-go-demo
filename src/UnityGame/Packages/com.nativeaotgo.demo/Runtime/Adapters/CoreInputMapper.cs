using Game.Domain.Core.Model;
using UnityEngine;

namespace NativeAotGoDemo.Unity.Adapters;

public static class CoreInputMapper
{
    public static StepInput FromUnity(
        ulong castId,
        ulong seedBase,
        int poseIndex,
        int sampleSlot,
        Transform lureTransform,
        Vector3 lureVelocity,
        float deltaTime,
        float tension,
        float waterTemp,
        int actionFlags)
    {
        return new StepInput(
            castId,
            seedBase,
            poseIndex,
            sampleSlot,
            deltaTime,
            Mathf.Max(0f, -lureTransform.position.y),
            lureVelocity.magnitude,
            tension,
            waterTemp,
            actionFlags);
    }
}
