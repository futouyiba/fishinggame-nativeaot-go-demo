using Game.Domain.Core.Simulation;
using NativeAotGoDemo.Unity.Adapters;
using UnityEngine;

namespace NativeAotGoDemo.Unity.Runtime;

public sealed class FishingRuntimeDriver : MonoBehaviour
{
    [SerializeField] private Transform lureTransform = default!;
    [SerializeField] private Vector3 lureVelocity = new(0f, 0f, 2.5f);
    [SerializeField] private float tension = 18f;
    [SerializeField] private float waterTemp = 19f;
    [SerializeField] private int actionFlags = 1;
    [SerializeField] private ulong castId = 10001;
    [SerializeField] private ulong seedBase = 20260318;
    [SerializeField] private int poseIndex = 3;
    [SerializeField] private int sampleSlot = 12;

    public StepDebugSnapshot LatestSnapshot { get; private set; } = new();

    private void Update()
    {
        if (lureTransform == null)
        {
            return;
        }

        var input = CoreInputMapper.FromUnity(
            castId,
            seedBase,
            poseIndex,
            sampleSlot,
            lureTransform,
            lureVelocity,
            Time.deltaTime,
            tension,
            waterTemp,
            actionFlags);

        var result = RealtimeStepCalculator.Step(in input);
        LatestSnapshot = StepDebugSnapshot.FromResult(result);
    }
}
