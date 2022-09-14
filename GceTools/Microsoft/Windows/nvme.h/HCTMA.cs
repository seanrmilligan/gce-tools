using USHORT = System.UInt16;

namespace Microsoft.Windows.nvme.h;

/// <summary>
/// Host Controlled Thermal Management Attributes (HCTMA)
/// </summary>
public readonly struct HCTMA
{
    private readonly USHORT _hctma;
    public USHORT Supported => (USHORT)(_hctma & 0b1);
    // the fifteen most significant bits are reserved
}