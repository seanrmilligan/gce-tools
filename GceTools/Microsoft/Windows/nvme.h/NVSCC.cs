using UCHAR = System.Byte;

namespace Microsoft.Windows.nvme.h;

/// <summary>
/// NVM Vendor Specific Command Configuration (NVSCC)
/// </summary>
public readonly struct NVSCC
{
    private readonly UCHAR _nvscc;
    public UCHAR CommandFormatInSpec => (UCHAR)(_nvscc & 0b1);
    // the seven most significant bits are reserved
}