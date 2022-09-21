namespace Microsoft.Native.nvme.h;

/// <summary>
/// Optional Admin Command Support (OACS)
/// </summary>
public readonly struct OACS
{
    private readonly UInt16 _oacs;
    public UInt16 SecurityCommands => (UInt16)(_oacs & 0b1);
    public UInt16 FormatNVM => (UInt16)((_oacs >> 1) & 0b1);
    public UInt16 FirmwareCommands => (UInt16)((_oacs >> 2) & 0b1);
    public UInt16 NamespaceCommands => (UInt16)((_oacs >> 3) & 0b1);
    public UInt16 DeviceSelfTest => (UInt16)((_oacs >> 4) & 0b1);
    public UInt16 Directives => (UInt16)((_oacs >> 5) & 0b1);
    // the ten most significant bits are reserved
}