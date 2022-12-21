// Copyright (c) third_party.Microsoft Corporation. All rights reserved.

using System.Runtime.InteropServices;

// Adapted from winioctl.h
namespace Microsoft.Native.winioctl.h
{
    // const int StoragePropertyQuerySize = 1024;
    
    /// <summary>
    /// Indicates the properties of a storage device or adapter to retrieve as
    /// the input buffer passed to the
    /// <see cref="T:Constant.IOCTL_STORAGE_QUERY_PROPERTY"/> control code.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct STORAGE_PROPERTY_QUERY
    {
        /// <summary>
        /// Indicates whether the caller is requesting a device descriptor, an
        /// adapter descriptor, a write cache property, a device unique ID
        /// (DUID), or the device identifiers provided in the device's SCSI
        /// vital product data (VPD) page.  For a list of the property IDs that
        /// can be assigned to this member, see
        /// <see cref="STORAGE_PROPERTY_ID"/>.
        /// </summary>
        public STORAGE_PROPERTY_ID PropertyId;
        
        /// <summary>
        /// Contains flags indicating the type of query to be performed as
        /// enumerated by the <see cref="STORAGE_QUERY_TYPE"/> enumeration.
        ///
        /// PropertyStandardQuery (0) Instructs the port driver to report a
        /// device descriptor, an adapter descriptor or a unique hardware device
        /// ID (DUID).
        /// PropertyExistsQuery (1) Instructs the port driver to report whether
        /// the descriptor is supported.
        /// </summary>
        public STORAGE_QUERY_TYPE QueryType;
        
        /// <summary>
        /// Contains an array of bytes that can be used to retrieve additional
        /// parameters for specific queries.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] AdditionalParameters;
    }
}