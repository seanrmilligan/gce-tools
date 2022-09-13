namespace Google.Cloud.Storage.Models;

/// <summary>
/// A managed (as opposed to native, unmanaged memory) wrapper of a
/// <see cref="Microsoft.Windows.winioctl.h.STORAGE_DEVICE_ID_DESCRIPTOR"/>.
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
}