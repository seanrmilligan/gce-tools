using ULONG = System.UInt32;

namespace Microsoft.Windows.nvme.h;

/// <summary>
/// Sanitize Capabilities (SANICAP)
/// </summary>
public readonly struct SANICAP
{
    private readonly ULONG _sanicap;
    
    /// <summary>
    /// Controller supports Crypto Erase Sanitize
    /// </summary>
    public ULONG CryptoErase => _sanicap & 0b1;
    
    /// <summary>
    /// Controller supports Block Erase Sanitize
    /// </summary>
    public ULONG BlockErase => (_sanicap >> 1) & 0b1;

    /// <summary>
    /// Controller supports Overwrite Santize
    /// </summary>
    public ULONG Overwrite => (_sanicap >> 2) & 0b1;
    
    // the twenty-nine most significant bits are reserved
}