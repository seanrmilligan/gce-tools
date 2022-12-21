// Copyright (c) third_party.Microsoft Corporation. All rights reserved.

// Adapted from winioctl.h
namespace Microsoft.Native.winioctl.h
{
    public enum STORAGE_PROTOCOL_NVME_DATA_TYPE
    {
        NVMeDataTypeUnknown,
        NVMeDataTypeIdentify,
        NVMeDataTypeLogPage,
        NVMeDataTypeFeature
    }
}