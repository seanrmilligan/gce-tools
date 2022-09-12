// Copyright (c) Microsoft Corporation. All rights reserved.

// Adapted from winioctl.h
namespace Microsoft.Windows.winioctl.h
{
    public enum STORAGE_PROTOCOL_NVME_DATA_TYPE
    {
        NVMeDataTypeUnknown,
        NVMeDataTypeIdentify,
        NVMeDataTypeLogPage,
        NVMeDataTypeFeature
    }
}