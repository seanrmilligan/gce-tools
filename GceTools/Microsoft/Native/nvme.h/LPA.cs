namespace Microsoft.Native.nvme.h;

/// <summary>
/// Log Page Attributes (LPA)
/// </summary>
public readonly struct LPA
{
    private readonly byte _lpa;
    public Byte SmartPagePerNamespace => (Byte)(_lpa & 0b1);
    public Byte CommandEffectsLog => (Byte)((_lpa >> 1) & 0b1);
    public Byte LogPageExtendedData => (Byte)((_lpa >> 2) & 0b1);
    public Byte TelemetrySupport => (Byte)((_lpa >> 3) & 0b1);
    // the four most significant bits are reserved
}