using USHORT = System.UInt16;

namespace Microsoft.Windows.nvme.h;

/// <summary>
/// Optional NVM Command Support (ONCS)
/// </summary>
public readonly struct ONCS
{
    private readonly USHORT _oncs;
    public USHORT Compare => (USHORT)(_oncs & 0b1);
    public USHORT WriteUncorrectable => (USHORT)((_oncs >> 1) & 0b1);
    public USHORT DatasetManagement => (USHORT)((_oncs >> 2) & 0b1);
    public USHORT WriteZeroes => (USHORT)((_oncs >> 3) & 0b1);
    public USHORT FeatureField => (USHORT)((_oncs >> 4) & 0b1);
    public USHORT Reservations => (USHORT)((_oncs >> 5) & 0b1);
    public USHORT Timestamp => (USHORT)((_oncs >> 6) & 0b1);
    // the nine most significant bits are reserved
}