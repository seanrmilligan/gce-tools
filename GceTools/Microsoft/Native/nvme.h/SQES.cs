namespace Microsoft.Native.nvme.h;

/// <summary>
/// Submission Queue Entry Size (SQES)
/// </summary>
public readonly struct SQES
{
    private readonly Byte _sqes;
    
    /// <summary>
    /// The value is in bytes and is reported as a power of two (2^n).
    /// </summary>
    public Byte RequiredEntrySize => (Byte)(_sqes & 0b1111);

    /// <summary>
    /// This value is larger than or equal to the required SQ entry size. The
    /// value is in bytes and is reported as a power of two (2^n).
    /// </summary>
    public Byte MaxEntrySize => (Byte)((_sqes >> 4) & 0b1111);
}