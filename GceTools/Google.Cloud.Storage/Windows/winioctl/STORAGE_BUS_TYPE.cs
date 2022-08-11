// Copyright (c) Microsoft Corporation. All rights reserved.
// https://docs.microsoft.com/en-us/windows/win32/api/winioctl/ne-winioctl-storage_bus_type
namespace Google.Cloud.Storage.Windows.winioctl
{
    /// <summary>
    /// Specifies the various types of storage buses.
    /// </summary>
    public enum STORAGE_BUS_TYPE : byte
    {
        /// <summary>
        /// Value: 0x00
        /// Unknown bus type.
        /// </summary>
        BusTypeUnknown,
        
        /// <summary>
        /// SCSI bus.
        /// </summary>
        BusTypeScsi,
        
        /// <summary>
        /// ATAPI bus.
        /// </summary>
        BusTypeAtapi,
        
        /// <summary>
        /// ATA bus.
        /// </summary>
        BusTypeAta,
        
        /// <summary>
        /// IEEE-1394 bus.
        /// </summary>
        BusType1394,
        
        /// <summary>
        /// SSA bus.
        /// </summary>
        BusTypeSsa,
        
        /// <summary>
        /// Fibre Channel bus.
        /// </summary>
        BusTypeFibre,
        
        /// <summary>
        /// USB bus.
        /// </summary>
        BusTypeUsb,
        
        /// <summary>
        /// RAID bus.
        /// </summary>
        BusTypeRAID,
        
        /// <summary>
        /// 
        /// </summary>
        BusTypeiScsi,
        
        /// <summary>
        /// Serial Attached SCSI (SAS) bus.
        /// Windows Server 2003:  This is not supported before Windows Server 2003 with SP1.
        /// </summary>
        BusTypeSas,
        
        /// <summary>
        /// SATA bus.
        /// Windows Server 2003:  This is not supported before Windows Server 2003 with SP1.
        /// </summary>
        BusTypeSata,
        
        /// <summary>
        /// 
        /// </summary>
        BusTypeSd,
        
        /// <summary>
        /// 
        /// </summary>
        BusTypeMmc,
        
        /// <summary>
        /// 
        /// </summary>
        BusTypeVirtual,
        
        /// <summary>
        /// 
        /// </summary>
        BusTypeFileBackedVirtual,
        
        /// <summary>
        /// 
        /// </summary>
        BusTypeSpaces,
        
        /// <summary>
        /// 
        /// </summary>
        BusTypeNvme,
        
        /// <summary>
        /// 
        /// </summary>
        BusTypeSCM,
        
        /// <summary>
        /// 
        /// </summary>
        BusTypeUfs,
        
        /// <summary>
        /// 
        /// </summary>
        BusTypeMax,
        
        /// <summary>
        /// Value: 0x7F
        /// </summary>
        BusTypeMaxReserved = 0x7F
    }
}