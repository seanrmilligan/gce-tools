using System.Runtime.InteropServices;

namespace Google.Cloud.Storage.Models;

[StructLayout(LayoutKind.Explicit, Size = 4096)]
public readonly struct Page
{
    [FieldOffset(0)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
    public readonly byte[] Bytes;
}