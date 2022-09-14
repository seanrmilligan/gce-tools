using UCHAR = System.Byte;

namespace Microsoft.Windows.nvme.h;

/// <summary>
/// Autonomous Power State Transition Attributes (APSTA)
/// </summary>
public readonly struct APSTA
{
    private readonly byte _apsta;
    public UCHAR Supported => (UCHAR)(_apsta & 0b1);
    // the seven most significant bits are reserved
}