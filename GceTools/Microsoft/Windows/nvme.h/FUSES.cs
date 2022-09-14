using USHORT = System.UInt16;

namespace Microsoft.Windows.nvme.h;

/// <summary>
/// Fused Operation Support (FUSES)
/// </summary>
public readonly struct FUSES
{
    private readonly USHORT _fuses;
    public USHORT CompareAndWrite => (USHORT)(_fuses & 0b1);
    // the fifteen most significant bits are reserved
}