namespace Microsoft.Native.nvme.h;

/// <summary>
/// SGL Support (SGLS)
/// </summary>
public readonly struct SGLS
{
    private readonly UInt32 _sgls;
    private UInt32 SGLSupported => _sgls & 0b1;
    // the next fifteen bits are reserved
    public UInt32 BitBucketDescrSupported => (_sgls >> 16) & 0b1;
    public UInt32 ByteAlignedContiguousPhysicalBuffer => (_sgls >> 17) & 0b1;
    public UInt32 SGLLengthLargerThanDataLength => (_sgls >> 18) & 0b1;
    // the thirteen most significant bits are reserved
}