namespace Microsoft.Native.nvme.h;

/// <summary>
/// Replay Protected Memory Block Support (RPMBS)
/// </summary>
public readonly struct RPMBS
{
    private readonly UInt32 _rpmbs;
    
    /// <summary>
    /// Number of RPMB Units
    /// </summary>
    public UInt32 RPMBUnitCount => _rpmbs & 0b111;
    
    /// <summary>
    /// Authentication Method
    /// </summary>
    public UInt32 AuthenticationMethod => (_rpmbs >> 3) & 0b111;
    
    // the next ten bits are reserved

    /// <summary>
    /// Total Size: in 128KB units.
    /// </summary>
    public UInt32 TotalSize => (_rpmbs >> 16) & 0xFF;
    
    /// <summary>
    /// Access Size: in 512B units.
    /// </summary>
    public UInt32 AccessSize => (_rpmbs >> 24) & 0xFF;
}