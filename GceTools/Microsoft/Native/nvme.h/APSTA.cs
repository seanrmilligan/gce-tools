using System;

namespace Microsoft.Native.nvme.h;

/// <summary>
/// Autonomous Power State Transition Attributes (APSTA)
/// </summary>
public readonly struct APSTA
{
    private readonly byte _apsta;
    public Byte Supported => (Byte)(_apsta & 0b1);
    // the seven most significant bits are reserved
}