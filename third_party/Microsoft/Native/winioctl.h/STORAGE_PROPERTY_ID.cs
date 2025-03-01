﻿// Copyright (c) third_party.Microsoft Corporation. All rights reserved.

// Adapted from winioctl.h
namespace Microsoft.Native.winioctl.h
{
    /// <summary>Enumerates the possible values of the PropertyId member of the
    /// <see cref="STORAGE_PROPERTY_QUERY"/> structure passed as input to the
    /// <see cref="T:Constant.IOCTL_STORAGE.QUERY_PROPERTY"/> request to retrieve the properties of a storage device or adapter.</summary>
    public enum STORAGE_PROPERTY_ID : int
    {
        /// <summary>
        /// Indicates that the caller is querying for the device descriptor.
        /// </summary>
        StorageDeviceProperty = 0,
        
        /// <summary>
        /// Indicates that the caller is querying for the adapter descriptor.
        /// </summary>
        StorageAdapterProperty = 1,
        
        /// <summary>
        /// Indicates that the caller is querying for the device identifiers
        /// provided with the SCSI vital product data pages.
        /// </summary>
        StorageDeviceIdProperty = 2,
        /// <summary>Indicates that the caller is querying for the unique device identifiers.</summary>
        StorageDeviceUniqueIdProperty = 3,
        /// <summary>Indicates that the caller is querying for the write cache property.</summary>
        StorageDeviceWriteCacheProperty = 4,
        /// <summary>Indicates that the caller is querying for the miniport driver descriptor.</summary>
        StorageMiniportProperty = 5,
        /// <summary>Indicates that the caller is querying for the access alignment descriptor.</summary>
        StorageAccessAlignmentProperty = 6,
        /// <summary>Indicates that the caller is querying for the seek penalty descriptor.</summary>
        StorageDeviceSeekPenaltyProperty = 7,
        /// <summary>Indicates that the caller is querying for the trim descriptor.</summary>
        StorageDeviceTrimProperty = 8,
        /// <summary>Indicates that the caller is querying for the write aggregation property.</summary>
        StorageDeviceWriteAggregationProperty = 9,
        /// <summary>This value is reserved.</summary>
        StorageDeviceDeviceTelemetryProperty = 10,
        /// <summary>Indicates that the caller is querying for the logical block provisioning descriptor, usually to detect whether the storage system uses thin provisioning.</summary>
        StorageDeviceLBProvisioningProperty = 11,
        /// <summary>Indicates that the caller is querying for the power optical disk drive descriptor.</summary>
        StorageDevicePowerProperty = 12,
        /// <summary>Indicates that the caller is querying for the write offload descriptor.</summary>
        StorageDeviceCopyOffloadProperty = 13,
        /// <summary>Indicates that the caller is querying for the device resiliency descriptor.</summary>
        StorageDeviceResiliencyProperty = 14,
        StorageDeviceMediumProductType = 15,
        StorageAdapterRpmbProperty = 16,
        StorageAdapterCryptoProperty = 17,
        StorageDeviceIoCapabilityProperty = 48,
        StorageAdapterProtocolSpecificProperty,
        StorageDeviceProtocolSpecificProperty,
        StorageAdapterTemperatureProperty,
        StorageDeviceTemperatureProperty,
        StorageAdapterPhysicalTopologyProperty,
        StorageDevicePhysicalTopologyProperty,
        StorageDeviceAttributesProperty,
        StorageDeviceManagementStatus,
        StorageAdapterSerialNumberProperty,
        StorageDeviceLocationProperty,
        StorageDeviceNumaProperty,
        StorageDeviceZonedDeviceProperty,
        StorageDeviceUnsafeShutdownCount,
        StorageDeviceEnduranceProperty,
        StorageDeviceLedStateProperty,
        StorageDeviceSelfEncryptionProperty,
        StorageFruIdProperty
    }
}