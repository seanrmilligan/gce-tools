using ULONG = System.UInt32;

namespace Microsoft.Windows.nvme.h;

/// <summary>
/// SGL Support (SGLS)
/// </summary>
public readonly struct SGLS
{
    private readonly ULONG _sgls;
    private ULONG SGLSupported => _sgls & 0b1;
    // the next fifteen bits are reserved
    public ULONG BitBucketDescrSupported => (_sgls >> 16) & 0b1;
    public ULONG ByteAlignedContiguousPhysicalBuffer => (_sgls >> 17) & 0b1;
    public ULONG SGLLengthLargerThanDataLength => (_sgls >> 18) & 0b1;
    // the thirteen most significant bits are reserved
}