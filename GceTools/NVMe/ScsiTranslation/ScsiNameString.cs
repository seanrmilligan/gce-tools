using System.Runtime.InteropServices;

namespace NVMe;

/// <summary>
/// Represents a translation of an NVMe device into a SCSI Name String as
/// defined by NVM Express: SCSI Translation Reference, Revision 1.5, Section
/// 6.1.4.4.1.3
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 68)]
public struct ScsiNameString
{
    [FieldOffset(0)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] PCIVendorId;
    
    [FieldOffset(4)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
    public byte[] ModelNumber;
    
    [FieldOffset(44)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] NamespaceId;

    [FieldOffset(48)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
    public byte[] SerialNumber;
}