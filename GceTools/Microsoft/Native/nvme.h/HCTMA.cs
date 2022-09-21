namespace Microsoft.Native.nvme.h;

/// <summary>
/// Host Controlled Thermal Management Attributes (HCTMA)
/// </summary>
public readonly struct HCTMA
{
    private readonly UInt16 _hctma;
    public UInt16 Supported => (UInt16)(_hctma & 0b1);
    // the fifteen most significant bits are reserved
}