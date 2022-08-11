// Copyright (c) Microsoft Corporation. All rights reserved.

using System;
using System.Runtime.InteropServices;

// Adapted from winioctl.h
namespace Google.Cloud.Storage.Windows.winioctl
{
    // Adapted from C:\Program Files (x86)\Windows Kits\10\Include\10.0.17763.0\um\winioctl.h
    //   WORD -> UInt16
    //   BYTE[1] -> Byte[]
    [StructLayout(LayoutKind.Sequential)]
    public struct STORAGE_IDENTIFIER
    {

        public STORAGE_IDENTIFIER_CODE_SET CodeSet;
        public STORAGE_IDENTIFIER_TYPE Type;
        public UInt16 IdentifierSize;
        public UInt16 NextOffset;
        //
        // Add new fields here since existing code depends on
        // the above layout not changing.
        //
        public STORAGE_ASSOCIATION_TYPE Association;
        //
        // The identifier is a variable length array of bytes.
        //
        // pjh: The final variable-length array 'Identifiers' field is declared
        // so that it matches STORAGE_DEVICE_ID_DESCRIPTOR just above. In
        // particular the "MarshalAs(UnmanagedType.ByValArray)" annotation is
        // critical. Without the "UnmanagedType.ByValArray" annotation, calling
        // Marshal.PtrToStructure on this memory will lead to "Unhandled
        // Exception: System.AccessViolationException: Attempted to read or
        // write protected memory. Without the SizeConst annotation, calling
        // Marshal.Copy from the Identifier array will lead to
        // "System.ArgumentOutOfRangeException: Requested range extends past the
        // end of the array" because the runtime assumes a default length of 1
        // for the managed Byte[].
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public Byte[] Identifier;
    }
}