using System.Runtime.InteropServices;

namespace Game.NativeAot.Export.NativeTypes;

[StructLayout(LayoutKind.Sequential)]
public struct StepOutputNative
{
    public int ResultCode;
    public float BiteProb;
    public float WeightFactor;
    public float TensionDelta;
    public uint FishTypeId;
    public byte IsHit;
    public byte Reserved1;
    public ushort Reserved2;
}
