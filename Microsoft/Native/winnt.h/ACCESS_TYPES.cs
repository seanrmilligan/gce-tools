using System;

namespace Microsoft.Native.winnt.h;

[Flags]
public enum ACCESS_TYPES : uint
{
    /// <summary>
    /// Read
    /// </summary>
    GENERIC_READ = 0x80000000,
    
    /// <summary>
    /// Write
    /// </summary>
    GENERIC_WRITE = 0x40000000,
    
    /// <summary>
    /// Execute
    /// </summary>
    GENERIC_EXECUTE = 0x20000000,
    
    /// <summary>
    /// All
    /// </summary>
    GENERIC_ALL = 0x10000000,
}