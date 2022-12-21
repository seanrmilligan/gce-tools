// https://docs.microsoft.com/en-us/windows/win32/api/winioctl/ne-winioctl-storage_protocol_type
// Copyright (c) Microsoft Corporation. All rights reserved.

namespace Microsoft.Native.winioctl.h
{
    /// <summary>
    /// Specifies the protocol of a storage device.
    /// </summary>
    public enum STORAGE_PROTOCOL_TYPE
    {
        /// <summary>
        /// Unknown protocol type.
        /// </summary>
        ProtocolTypeUnknown = 0,
        
        /// <summary>
        /// SCSI protocol type.
        /// </summary>
        ProtocolTypeScsi = 1,
        
        /// <summary>
        /// ATA protocol type.
        /// </summary>
        ProtocolTypeAta = 2,
        
        /// <summary>
        /// NVMe protocol type.
        /// </summary>
        ProtocolTypeNvme = 3,
        
        /// <summary>
        /// SD protocol type.
        /// </summary>
        ProtocolTypeSd = 4,
        
        /// <summary>
        /// 
        /// </summary>
        ProtocolTypeUfs = 5,
        
        /// <summary>
        /// Value: 0x7E
        /// Vendor-specific protocol type.
        /// </summary>
        ProtocolTypeProprietary = 0x7E,
        
        /// <summary>
        /// Value: 0x7F
        /// Reserved.
        /// </summary>
        ProtocolTypeMaxReserved = 0x7F
    }
}