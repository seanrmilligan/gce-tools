namespace Microsoft.Native.nvme.h;

/// <summary>
/// Format NVM Attributes (FNA)
/// </summary>
public readonly struct FNA
{
    private readonly Byte _fna;
    public Byte FormatApplyToAll => (Byte)(_fna & 0b1);
    public Byte SecureEraseApplyToAll => (Byte)((_fna >> 1) & 0b1);
    public Byte CryptographicEraseSupported => (Byte)((_fna >> 2) & 0b1);
    // the five most significant bits are reserved
}