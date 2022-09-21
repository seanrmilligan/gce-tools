namespace Microsoft.Native.nvme.h;

/// <summary>
/// NVM Vendor Specific Command Configuration (NVSCC)
/// </summary>
public readonly struct NVSCC
{
    private readonly Byte _nvscc;
    public Byte CommandFormatInSpec => (Byte)(_nvscc & 0b1);
    // the seven most significant bits are reserved
}