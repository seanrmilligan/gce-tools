// Copyright (c) Microsoft Corporation. All rights reserved.
// https://docs.microsoft.com/en-us/windows/win32/api/winioctl/ns-winioctl-storage_device_id_descriptor
using System.Runtime.InteropServices;
using DWORD = System.UInt32;
using BYTE = System.Byte;

// Adapted from winioctl.h
namespace Google.Cloud.Storage.Windows.winioctl
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
    [StructLayout(LayoutKind.Sequential)]
    public struct STORAGE_DEVICE_ID_DESCRIPTOR
    {
        /// <summary>
        /// Contains the size of this structure, in bytes. The value of this
        /// member will change as members are added to the structure.
        /// </summary>
        public DWORD Version;
        
        /// <summary>
        /// Specifies the total size of the data returned, in bytes. This may
        /// include data that follows this structure.
        /// </summary>
        public DWORD Size;
        
        /// <summary>
        /// Contains the number of identifiers reported by the device in the
        /// Identifiers array.
        /// </summary>
        public DWORD NumberOfIdentifiers;
        
        /// <summary>
        /// Contains a variable-length array of identification descriptors
        /// (STORAGE_IDENTIFIER).
        /// </summary>
        // pjh: BUFFER_SIZE (512 bytes) is assumed to be sufficient here.
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public BYTE[] Identifiers;
        
        public override string ToString()
        {
            return string.Join("\n", new[]
            {
                $"Version:               {Version}",
                $"Size:                  {Size}",
                $"NumberOfIdentifiers:   {NumberOfIdentifiers}",
                $"Identifiers:           {System.Text.Encoding.ASCII.GetString(Identifiers)}"
            });
        }
    }
}