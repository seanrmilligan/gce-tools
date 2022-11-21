using System;

namespace Microsoft.Native.nvme.h;

/// <summary>
/// Firmware Updates (FRMW)
/// </summary>
public readonly struct FRMW
{
    private readonly byte _frmw;
    public Byte Slot1ReadOnly => (Byte)(_frmw & 0b1);
    public Byte SlotCount => (Byte)((_frmw >> 1) & 0b111);
    public Byte ActivationWithoutReset => (Byte)((_frmw >> 4) & 0b1);
    // the three most significant bits are reserved
}