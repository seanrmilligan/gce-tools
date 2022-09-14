using UCHAR = System.Byte;

namespace Microsoft.Windows.nvme.h;

public readonly struct CMIC
{
    private readonly byte _cmic;
    public UCHAR MultiPCIePorts => (UCHAR)(_cmic & 0b1);
    public UCHAR MultiControllers => (UCHAR)((_cmic >> 1) & 0b1);
    public UCHAR SRIOV => (UCHAR)((_cmic >> 2) & 0b1);
    // the five most significant bits are reserved
}