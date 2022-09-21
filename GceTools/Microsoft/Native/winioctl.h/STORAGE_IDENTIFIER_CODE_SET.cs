// Copyright (c) Microsoft Corporation. All rights reserved.

// Adapted from winioctl.h
namespace Microsoft.Native.winioctl.h
{
    public enum STORAGE_IDENTIFIER_CODE_SET : int
    {
        StorageIdCodeSetReserved = 0,
        StorageIdCodeSetBinary = 1,
        StorageIdCodeSetAscii = 2,
        StorageIdCodeSetUtf8 = 3
    }
}