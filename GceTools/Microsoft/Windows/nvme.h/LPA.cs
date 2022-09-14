using UCHAR = System.Byte;

namespace Microsoft.Windows.nvme.h;

/// <summary>
/// Log Page Attributes (LPA)
/// </summary>
public readonly struct LPA
{
    private readonly byte _lpa;
    public UCHAR SmartPagePerNamespace => (UCHAR)(_lpa & 0b1);
    public UCHAR CommandEffectsLog => (UCHAR)((_lpa >> 1) & 0b1);
    public UCHAR LogPageExtendedData => (UCHAR)((_lpa >> 2) & 0b1);
    public UCHAR TelemetrySupport => (UCHAR)((_lpa >> 3) & 0b1);
    // the four most significant bits are reserved
}