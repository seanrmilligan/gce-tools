namespace Microsoft.Windows.winnt.h;

[Flags]
public enum FILE_SHARE : uint
{
    /// <summary>
    /// Enables subsequent open operations on a file or device to request read access.
    /// Otherwise, other processes cannot open the file or device if they request read access.
    /// </summary>
    READ = 0x00000001,
    
    /// <summary>
    /// Enables subsequent open operations on a file or device to request write access.
    /// Otherwise, other processes cannot open the file or device if they request write access.
    /// </summary>
    WRITE = 0x00000002,
    
    /// <summary>
    /// Enables subsequent open operations on a file or device to request delete access.
    /// Otherwise, other processes cannot open the file or device if they request delete access.
    /// If this flag is not specified, but the file or device has been opened for delete access, the function fails.
    /// </summary>
    DELETE = 0x00000004,
}