using System;

namespace Microsoft.Native.nvme.h;

/// <summary>
/// Optional Async Event Support (OAES)
/// </summary>
public readonly struct OAES
{
    private readonly uint _oaes;
    // the eight least significant bits are reserved
    public UInt32 NamespaceAttributeChanged => (_oaes >> 8) & 0b1;
    public UInt32 FirmwareActivation => (_oaes >> 9) & 0b1;
    // the twenty-two most significant bits are reserved
}