﻿// Copyright (c) third_party.Microsoft Corporation. All rights reserved.

// Adapted from winioctl.h
namespace Microsoft.Native.winioctl.h
{
    public enum STORAGE_IDENTIFIER_TYPE : int
    {
        StorageIdTypeVendorSpecific = 0,
        StorageIdTypeVendorId = 1,
        StorageIdTypeEUI64 = 2,
        StorageIdTypeFCPHName = 3,
        StorageIdTypePortRelative = 4,
        StorageIdTypeTargetPortGroup = 5,
        StorageIdTypeLogicalUnitGroup = 6,
        StorageIdTypeMD5LogicalUnitIdentifier = 7,
        StorageIdTypeScsiNameString = 8
    }
}