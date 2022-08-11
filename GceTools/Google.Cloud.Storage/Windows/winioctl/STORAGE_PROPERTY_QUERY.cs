// Copyright (c) Microsoft Corporation. All rights reserved.

using System.Runtime.InteropServices;

// Adapted from winioctl.h
namespace Google.Cloud.Storage.Windows.winioctl
{
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
        /// vital product data (VPD) page.
        /// </summary>
        public STORAGE_PROPERTY_ID PropertyId;
        
        /// <summary>
        /// Contains flags indicating the type of query to be performed as
        /// enumerated by the <see cref="STORAGE_QUERY_TYPE"/> enumeration.
        /// </summary>
        public STORAGE_QUERY_TYPE QueryType;
        
        /// <summary>
        /// Contains an array of bytes that can be used to retrieve additional
        /// parameters for specific queries.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public byte[] AdditionalParameters;
    }
}