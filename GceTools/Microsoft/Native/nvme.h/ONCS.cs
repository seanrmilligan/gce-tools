namespace Microsoft.Native.nvme.h;

/// <summary>
/// Optional NVM Command Support (ONCS)
/// </summary>
public readonly struct ONCS
{
    private readonly UInt16 _oncs;
    public UInt16 Compare => (UInt16)(_oncs & 0b1);
    public UInt16 WriteUncorrectable => (UInt16)((_oncs >> 1) & 0b1);
    public UInt16 DatasetManagement => (UInt16)((_oncs >> 2) & 0b1);
    public UInt16 WriteZeroes => (UInt16)((_oncs >> 3) & 0b1);
    public UInt16 FeatureField => (UInt16)((_oncs >> 4) & 0b1);
    public UInt16 Reservations => (UInt16)((_oncs >> 5) & 0b1);
    public UInt16 Timestamp => (UInt16)((_oncs >> 6) & 0b1);
    // the nine most significant bits are reserved
}