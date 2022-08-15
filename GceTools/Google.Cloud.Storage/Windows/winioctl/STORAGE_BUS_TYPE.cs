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
        BusTypeUnknown = 0,
        
        /// <summary>
        /// SCSI bus.
        /// </summary>
        BusTypeScsi = 1,
        
        /// <summary>
        /// ATAPI bus.
        /// </summary>
        BusTypeAtapi = 2,
        
        /// <summary>
        /// ATA bus.
        /// </summary>
        BusTypeAta = 3,
        
        /// <summary>
        /// IEEE-1394 bus.
        /// </summary>
        BusType1394 = 4,
        
        /// <summary>
        /// SSA bus.
        /// </summary>
        BusTypeSsa = 5,
        
        /// <summary>
        /// Fibre Channel bus.
        /// </summary>
        BusTypeFibre = 6,
        
        /// <summary>
        /// USB bus.
        /// </summary>
        BusTypeUsb = 7,
        
        /// <summary>
        /// RAID bus.
        /// </summary>
        BusTypeRAID = 8,
        
        /// <summary>
        /// 
        /// </summary>
        BusTypeiScsi = 9,
        
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