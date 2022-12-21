using System.Runtime.InteropServices;

namespace NVMe.ScsiTranslation.v1_5;

/// <summary>
/// Represents a translation of an third_party.NVMe v1.1+ compliant device into a SCSI Name
/// String as defined by NVM Express: SCSI Translation Reference, Revision 1.5,
/// Section 6.1.4.4.1.1
/// </summary>
/// <remarks>
/// This variant is used when the third_party.NVMe EUI64 Field is zero and the third_party.NVMe NGUID
/// Field is non-zero. This variant may also be used when both fields are
/// non-zero.
/// </remarks>
[StructLayout(LayoutKind.Explicit)]
public struct NguidBasedScsiNameString
{
    /// <summary>
    /// A 36 byte UTF-8 character field comprised of the four UTF-8 characters
    /// 'eui.' concatenated with UTF-8 representation of the 32 hexadecimal
    /// digits corresponding to the 128 bit NGUID field of the Identify
    /// Namespace Data Structure.
    /// </summary>
    /// <remarks>
    /// The first hexadecimal digit shall be the most significant four bits of
    /// the first byte (i.e., most significant byte) of the NGUID field.
    /// </remarks>
    [FieldOffset(0)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
    public byte[] Nguid;
}