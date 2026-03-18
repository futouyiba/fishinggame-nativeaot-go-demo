using System.Runtime.InteropServices;

namespace Game.NativeAot.Export.NativeTypes;

[StructLayout(LayoutKind.Sequential)]
public struct SessionStepInputNative
{
    public float Dt;
    public float LureDepth;
    public float LureSpeed;
    public int ActionFlags;
}
