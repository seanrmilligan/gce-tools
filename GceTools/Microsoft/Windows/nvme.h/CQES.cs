using UCHAR = System.Byte;

namespace Microsoft.Windows.nvme.h;

/// <summary>
/// Completion Queue Entry Size (CQES)
/// </summary>
public readonly struct CQES
{
    private readonly UCHAR _cqes;
    
    /// <summary>
    /// The value is in bytes and is reported as a power of two (2^n).
    /// </summary>
    public UCHAR RequiredEntrySize => (UCHAR)(_cqes & 0b1111);

    /// <summary>
    /// This value is larger than or equal to the required CQ entry size. The
    /// value is in bytes and is reported as a power of two (2^n).
    /// </summary>
    public UCHAR MaxEntrySize => (UCHAR)((_cqes >> 4) & 0b1111);
}