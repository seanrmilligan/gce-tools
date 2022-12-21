using System;

namespace Microsoft.Native.storduid.h;

public struct STORAGE_DEVICE_UNIQUE_IDENTIFIER
{
    /// <summary>
    /// The version of the DUID.
    /// </summary>
    public UInt32 Version;
    
    /// <summary>
    /// The size, in bytes, of the identifier header and the identifiers (IDs)
    /// that follow the header.
    /// </summary>
    public UInt32 Size;
    
    /// <summary>
    /// The offset, in bytes, from the beginning of the header to the device ID
    /// descriptor (STORAGE_DEVICE_ID_DESCRIPTOR). The device ID descriptor
    /// contains the IDs that are extracted from page 0x83 of the device's
    /// vital product data (VPD).
    /// </summary>
    public UInt32 StorageDeviceIdOffset;
    public UInt32 StorageDeviceOffset;
    public UInt32 DriveLayoutSignatureOffset;
}