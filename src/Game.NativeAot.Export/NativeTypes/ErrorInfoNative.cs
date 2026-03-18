using System.Runtime.InteropServices;

namespace Game.NativeAot.Export.NativeTypes;

[StructLayout(LayoutKind.Sequential)]
public struct ErrorInfoNative
{
    public int Code;
    public int Detail;
}
