// Copyright (c) Microsoft Corporation. All rights reserved.

// Adapted from winioctl.h
namespace Google.Cloud.Storage.Windows.winioctl
{
    public enum STORAGE_IDENTIFIER_CODE_SET
    {
        StorageIdCodeSetReserved = 0,
        StorageIdCodeSetBinary = 1,
        StorageIdCodeSetAscii = 2,
        StorageIdCodeSetUtf8 = 3
    }
}