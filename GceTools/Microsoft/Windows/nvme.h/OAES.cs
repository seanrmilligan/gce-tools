using ULONG = System.UInt32;

namespace Microsoft.Windows.nvme.h;

/// <summary>
/// Optional Async Event Support (OAES)
/// </summary>
public readonly struct OAES
{
    private readonly uint _oaes;
    // the eight least significant bits are reserved
    public ULONG NamespaceAttributeChanged => (_oaes >> 8) & 0b1;
    public ULONG FirmwareActivation => (_oaes >> 9) & 0b1;
    // the twenty-two most significant bits are reserved
}