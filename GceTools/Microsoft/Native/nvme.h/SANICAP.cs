using System;

namespace Microsoft.Native.nvme.h;

/// <summary>
/// Sanitize Capabilities (SANICAP)
/// </summary>
public readonly struct SANICAP
{
    private readonly UInt32 _sanicap;
    
    /// <summary>
    /// Controller supports Crypto Erase Sanitize
    /// </summary>
    public UInt32 CryptoErase => _sanicap & 0b1;
    
    /// <summary>
    /// Controller supports Block Erase Sanitize
    /// </summary>
    public UInt32 BlockErase => (_sanicap >> 1) & 0b1;

    /// <summary>
    /// Controller supports Overwrite Santize
    /// </summary>
    public UInt32 Overwrite => (_sanicap >> 2) & 0b1;
    
    // the twenty-nine most significant bits are reserved
}