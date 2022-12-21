namespace NVMe.ScsiTranslation.v1_5;

/// <summary>
/// Represents a translation of an NVMe v1.1+ compliant device into a SCSI Name
/// String as defined by NVM Express: SCSI Translation Reference, Revision 1.5,
/// Section 6.1.4.4.1.1
/// </summary>
/// <remarks>
/// This variant is used when the NVMe EUI64 Field is non-zero and the NVMe
/// NGUID Field is zero. This variant may also be used when both fields are
/// non-zero.
/// </remarks>
public class Eui64BasedScsiNameString
{
    /// <summary>
    /// A 20 byte UTF-8 character field comprised of the four UTF-8 characters
    /// 'eui.' concatenated with UTF-8 representation of the 16 hexadecimal
    /// digits corresponding to the 64 bit EUI64 field of the Identify Namespace
    /// Data Structure.
    /// </summary>
    /// <remarks>
    /// The first hexadecimal digit shall be the most significant four bits of
    /// the first byte (i.e., most significant byte) of the EUI-64 field.
    /// </remarks>
    public byte[] Eui64;
    
}