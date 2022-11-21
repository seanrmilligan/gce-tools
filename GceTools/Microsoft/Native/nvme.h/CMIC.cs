using System;

namespace Microsoft.Native.nvme.h;

public readonly struct CMIC
{
    private readonly byte _cmic;
    public Byte MultiPCIePorts => (Byte)(_cmic & 0b1);
    public Byte MultiControllers => (Byte)((_cmic >> 1) & 0b1);
    public Byte SRIOV => (Byte)((_cmic >> 2) & 0b1);
    // the five most significant bits are reserved
}