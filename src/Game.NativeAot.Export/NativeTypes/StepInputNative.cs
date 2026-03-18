using System.Runtime.InteropServices;

namespace Game.NativeAot.Export.NativeTypes;

[StructLayout(LayoutKind.Sequential)]
public struct StepInputNative
{
    public ulong CastId;
    public ulong SeedBase;
    public int PoseIndex;
    public int SampleSlot;
    public float Dt;
    public float LureDepth;
    public float LureSpeed;
    public float Tension;
    public float WaterTemp;
    public int ActionFlags;
}
