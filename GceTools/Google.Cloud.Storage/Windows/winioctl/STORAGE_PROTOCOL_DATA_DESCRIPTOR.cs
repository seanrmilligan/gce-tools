// Copyright (c) Microsoft Corporation. All rights reserved.

using System.Runtime.InteropServices;
using DWORD = System.UInt32;

// Adapted from winioctl.h
namespace Google.Cloud.Storage.Windows.winioctl
{
    [StructLayout(LayoutKind.Sequential)]
    public struct STORAGE_PROTOCOL_DATA_DESCRIPTOR
    {
        DWORD                          Version;
        DWORD                          Size;
        STORAGE_PROTOCOL_SPECIFIC_DATA ProtocolSpecificData;
    } 
}