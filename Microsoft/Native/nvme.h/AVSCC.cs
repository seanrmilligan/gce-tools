using System;

namespace Microsoft.Native.nvme.h;

/// <summary>
/// Admin Vendor Specific Command Configuration (AVSCC)
/// </summary>
public readonly struct AVSCC
{
    private readonly byte _avscc;
    public Byte CommandFormatInSpec => (Byte)(_avscc & 0b1);
    // the seven most significant bits are reserved
}