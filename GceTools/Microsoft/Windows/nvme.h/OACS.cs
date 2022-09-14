using USHORT = System.UInt16;

namespace Microsoft.Windows.nvme.h;

/// <summary>
/// Optional Admin Command Support (OACS)
/// </summary>
public readonly struct OACS
{
    private readonly USHORT _oacs;
    public USHORT SecurityCommands => (USHORT)(_oacs & 0b1);
    public USHORT FormatNVM => (USHORT)((_oacs >> 1) & 0b1);
    public USHORT FirmwareCommands => (USHORT)((_oacs >> 2) & 0b1);
    public USHORT NamespaceCommands => (USHORT)((_oacs >> 3) & 0b1);
    public USHORT DeviceSelfTest => (USHORT)((_oacs >> 4) & 0b1);
    public USHORT Directives => (USHORT)((_oacs >> 5) & 0b1);
    // the ten most significant bits are reserved
}