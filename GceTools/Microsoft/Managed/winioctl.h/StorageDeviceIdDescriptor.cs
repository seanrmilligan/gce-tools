using Microsoft.Native.winioctl.h;

namespace Microsoft.Managed.winioctl.h;

/// <summary>
/// A managed (as opposed to native, unmanaged memory) wrapper of a
/// <see cref="STORAGE_DEVICE_ID_DESCRIPTOR"/>.
/// </summary>
public class StorageDeviceIdDescriptor
{
    /// <summary>
    /// Contains the size of this structure, in bytes. The value of this
    /// member will change as members are added to the structure.
    /// </summary>
    public uint Version;
        
    /// <summary>
    /// Specifies the total size of the data returned, in bytes. This may
    /// include data that follows this structure.
    /// </summary>
    public uint Size;
        
    /// <summary>
    /// Contains the number of identifiers reported by the device in the
    /// Identifiers array.
    /// </summary>
    public uint NumberOfIdentifiers;
        
    /// <summary>
    /// Contains a variable-length array of identification descriptors
    /// (STORAGE_IDENTIFIER).
    /// </summary>
    public StorageIdentifier[] Identifiers { get; set; }
    
    public override string ToString()
    {
        string indent = Environment.NewLine + "  ";
        return string.Join(Environment.NewLine, new[]
        {
            $"Version:               {Version}",
            $"Size:                  {Size}",
            $"NumberOfIdentifiers:   {NumberOfIdentifiers}",
            $"Identifiers:",
            $"  {string.Join(indent, Identifiers.Select(id => id.ToString().Replace(Environment.NewLine, indent)).ToArray())}"
        });
    }
}