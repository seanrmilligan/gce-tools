// Copyright (c) third_party.Microsoft Corporation. All rights reserved.
// https://docs.microsoft.com/en-us/windows/win32/api/winioctl/ns-winioctl-storage_device_id_descriptor

using System;
using System.Runtime.InteropServices;

// Adapted from winioctl.h
namespace Microsoft.Native.winioctl.h
{
    /// <summary>
    /// Used with the <see cref="T:Constant.IOCTL_STORAGE.QUERY_PROPERTY"/>
    /// control code request to retrieve the device ID descriptor data for a
    /// device.
    /// </summary>
    /// <remarks>
    /// The device ID descriptor consists of an array of device IDs taken from
    /// the SCSI-3 vital product data (VPD) page 0x83 that was retrieved during
    /// discovery.
    /// </remarks>
    [StructLayout(LayoutKind.Explicit)]
    public struct STORAGE_DEVICE_ID_DESCRIPTOR
    {
        /// <summary>
        /// Contains the size of this structure, in bytes. The value of this
        /// member will change as members are added to the structure.
        /// </summary>
        [FieldOffset(0)]
        public UInt32 Version;
        
        /// <summary>
        /// Specifies the total size of the data returned, in bytes. This may
        /// include data that follows this structure.
        /// </summary>
        [FieldOffset(4)]
        public UInt32 Size;
        
        /// <summary>
        /// Contains the number of identifiers reported by the device in the
        /// Identifiers array.
        /// </summary>
        [FieldOffset(8)]
        public UInt32 NumberOfIdentifiers;
        
        /// <summary>
        /// Contains a variable-length array of identification descriptors
        /// (STORAGE_IDENTIFIER).
        /// </summary>
        [FieldOffset(12)]
        public Byte Identifiers;
        
        public override string ToString()
        {
            return string.Join("\n", new[]
            {
                $"Version:               {Version}",
                $"Size:                  {Size}",
                $"NumberOfIdentifiers:   {NumberOfIdentifiers}"
            });
        }
    }
}