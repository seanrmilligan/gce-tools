// Copyright (c) Microsoft Corporation. All rights reserved.

using System.Runtime.InteropServices;

using UCHAR = System.Byte;
using USHORT = System.UInt16;

namespace Google.Cloud.Storage.Windows.winioctl
{
    [StructLayout(LayoutKind.Sequential)]
    public struct STORAGE_IDENTIFIER
    {
        /// <summary>
        /// Specifies the code set used by a SCSI identification descriptor to
        /// identify a logical unit.
        /// </summary>
        public STORAGE_IDENTIFIER_CODE_SET CodeSet;
        
        /// <summary>
        /// Contains an enumerator value of type
        /// <see cref="STORAGE_IDENTIFIER_TYPE"/> that indicates the identifier
        /// type.
        /// </summary>
        public STORAGE_IDENTIFIER_TYPE Type;
        
        /// <summary>
        /// Specifies the size in bytes of the identifier.
        /// </summary>
        public USHORT IdentifierSize;
        
        /// <summary>
        /// Specifies the offset in bytes from the current descriptor to the
        /// next descriptor.
        /// </summary>
        public USHORT NextOffset;
        
        /// <summary>
        /// Contains an enumerator value of type
        /// <see cref="STORAGE_ASSOCIATION_TYPE"/> that indicates whether the
        /// descriptor identifies a device or a port.
        /// </summary>
        public STORAGE_ASSOCIATION_TYPE Association;

        /// <summary>
        /// Contains the identifier associated with this descriptor.
        /// </summary>
        /// <remarks>
        /// The identifier is a variable length array of bytes.
        ///
        /// pjh: The final variable-length array 'Identifiers' field is declared
        /// so that it matches STORAGE_DEVICE_ID_DESCRIPTOR just above. In
        /// particular the "MarshalAs(UnmanagedType.ByValArray)" annotation is
        /// critical. Without the "UnmanagedType.ByValArray" annotation, calling
        /// Marshal.PtrToStructure on this memory will lead to "Unhandled
        /// Exception: System.AccessViolationException: Attempted to read or
        /// write protected memory. Without the SizeConst annotation, calling
        /// Marshal.Copy from the Identifier array will lead to
        /// "System.ArgumentOutOfRangeException: Requested range extends past the
        /// end of the array" because the runtime assumes a default length of 1
        /// for the managed Byte[].
        /// </remarks>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public UCHAR[] Identifier;
    }
}