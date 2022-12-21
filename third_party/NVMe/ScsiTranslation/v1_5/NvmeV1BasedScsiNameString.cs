using System.Linq;
using System.Runtime.InteropServices;

namespace NVMe.ScsiTranslation.v1_5;

/// <summary>
/// Represents a translation of an third_party.NVMe v1.0 compliant device into a SCSI Name
/// String as defined by NVM Express: SCSI Translation Reference, Revision 1.5,
/// Section 6.1.4.4.1.3
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 68)]
public readonly struct NvmeV1BasedScsiNameString
{
    [FieldOffset(0)]
    private readonly byte _vid0;
    
    [FieldOffset(1)]
    private readonly byte _vid1;
    
    [FieldOffset(2)]
    private readonly byte _vid2;
    
    [FieldOffset(3)]
    private readonly byte _vid3;

    /// <summary>
    /// PCI Vendor ID (UTF-8 representation)
    /// </summary>
    /// <remarks>
    /// bytes 01:00 of Identify Controller converted to 4 UTF-8 characters
    /// </remarks>
    public byte[] PciVendorId => new[] { _vid0, _vid1, _vid2, _vid3 };

    [FieldOffset(4)]
    private readonly byte _mn0;
    
    [FieldOffset(5)]
    private readonly byte _mn1;
    
    [FieldOffset(6)]
    private readonly byte _mn2;
    
    [FieldOffset(7)]
    private readonly byte _mn3;
    
    [FieldOffset(8)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
    private readonly byte[] _mn4;
    
    /// <summary>
    /// Model Number
    /// </summary>
    /// <remarks>
    /// bytes 63:24 of Identify Controller data structure
    /// </remarks>
    public byte[] ModelNumber => new [] { _mn0, _mn1, _mn2, _mn3, }
        .Concat(_mn4)
        .ToArray();

    [FieldOffset(44)]
    private readonly byte _nsid0;
    
    [FieldOffset(45)]
    private readonly byte _nsid1;
    
    [FieldOffset(46)]
    private readonly byte _nsid2;
    
    [FieldOffset(47)]
    private readonly byte _nsid3;

    /// <summary>
    /// Namespace ID (UTF-8 representation)
    /// </summary>
    public byte[] NamespaceId => new[] { _nsid0, _nsid1, _nsid2, _nsid3 };

    /// <summary>
    /// Serial Number
    /// </summary>
    /// <remarks>
    /// bytes 23:04 of Identify Controller data structure
    /// </remarks>
    [FieldOffset(48)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
    public readonly byte[] SerialNumber;
}