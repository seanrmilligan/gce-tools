using UCHAR = System.Byte;

namespace Microsoft.Windows.nvme.h;

/// <summary>
/// Admin Vendor Specific Command Configuration (AVSCC)
/// </summary>
public readonly struct AVSCC
{
    private readonly byte _avscc;
    public UCHAR CommandFormatInSpec => (UCHAR)(_avscc & 0b1);
    // the seven most significant bits are reserved
}