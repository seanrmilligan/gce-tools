// Copyright (c) Microsoft Corporation. All rights reserved.

// Adapted from winioctl.h
namespace Google.Cloud.Storage.Windows.winioctl
{
    public enum STORAGE_PROTOCOL_NVME_DATA_TYPE
    {
        NVMeDataTypeUnknown,
        NVMeDataTypeIdentify,
        NVMeDataTypeLogPage,
        NVMeDataTypeFeature
    }
}