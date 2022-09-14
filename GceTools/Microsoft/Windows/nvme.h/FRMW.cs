using UCHAR = System.Byte;

namespace Microsoft.Windows.nvme.h;

/// <summary>
/// Firmware Updates (FRMW)
/// </summary>
public readonly struct FRMW
{
    private readonly byte _frmw;
    public UCHAR Slot1ReadOnly => (UCHAR)(_frmw & 0b1);
    public UCHAR SlotCount => (UCHAR)((_frmw >> 1) & 0b111);
    public UCHAR ActivationWithoutReset => (UCHAR)((_frmw >> 4) & 0b1);
    // the three most significant bits are reserved
}