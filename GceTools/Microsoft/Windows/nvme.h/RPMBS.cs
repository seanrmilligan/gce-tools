using ULONG = System.UInt32;

namespace Microsoft.Windows.nvme.h;

/// <summary>
/// Replay Protected Memory Block Support (RPMBS)
/// </summary>
public readonly struct RPMBS
{
    private readonly ULONG _rpmbs;
    
    /// <summary>
    /// Number of RPMB Units
    /// </summary>
    public ULONG RPMBUnitCount => _rpmbs & 0b111;
    
    /// <summary>
    /// Authentication Method
    /// </summary>
    public ULONG AuthenticationMethod => (_rpmbs >> 3) & 0b111;
    
    // the next ten bits are reserved

    /// <summary>
    /// Total Size: in 128KB units.
    /// </summary>
    public ULONG TotalSize => (_rpmbs >> 16) & 0xFF;
    
    /// <summary>
    /// Access Size: in 512B units.
    /// </summary>
    public ULONG AccessSize => (_rpmbs >> 24) & 0xFF;
}