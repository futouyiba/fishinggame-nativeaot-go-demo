using System.Runtime.InteropServices;

namespace Game.NativeAot.Export.NativeTypes;

[StructLayout(LayoutKind.Sequential)]
public struct SessionConfigNative
{
    public ulong CastId;
    public ulong SeedBase;
    public int PoseIndex;
    public int InitialSampleSlot;
    public float InitialTension;
    public float WaterTemp;
}
