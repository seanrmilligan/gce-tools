using UCHAR = System.Byte;

namespace Microsoft.Windows.nvme.h;

/// <summary>
/// Format NVM Attributes (FNA)
/// </summary>
public readonly struct FNA
{
    private readonly UCHAR _fna;
    public UCHAR FormatApplyToAll => (UCHAR)(_fna & 0b1);
    public UCHAR SecureEraseApplyToAll => (UCHAR)((_fna >> 1) & 0b1);
    public UCHAR CryptographicEraseSupported => (UCHAR)((_fna >> 2) & 0b1);
    // the five most significant bits are reserved
}