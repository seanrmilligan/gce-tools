using System;

namespace Microsoft.Native.nvme.h;

/// <summary>
/// Fused Operation Support (FUSES)
/// </summary>
public readonly struct FUSES
{
    private readonly UInt16 _fuses;
    public UInt16 CompareAndWrite => (UInt16)(_fuses & 0b1);
    // the fifteen most significant bits are reserved
}