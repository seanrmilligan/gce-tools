using System.Runtime.InteropServices;

namespace Google.Cloud.Storage
{
    public struct Page
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
        public byte[] DataBuffer;
    }
}