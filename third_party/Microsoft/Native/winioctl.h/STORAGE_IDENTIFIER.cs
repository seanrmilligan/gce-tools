// Copyright (c) third_party.Microsoft Corporation. All rights reserved.

using System.Runtime.InteropServices;
using UCHAR = System.Byte;
using USHORT = System.UInt16;

namespace Microsoft.Native.winioctl.h
{
    /// <summary>
    /// The STORAGE_IDENTIFIER structure represents a SCSI identification
    /// descriptor.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct STORAGE_IDENTIFIER
    {
        /// <summary>
        /// Specifies the code set used by a SCSI identification descriptor to
        /// identify a logical unit.
        /// </summary>
        [FieldOffset(0)]
        public STORAGE_IDENTIFIER_CODE_SET CodeSet;
        
        /// <summary>
        /// Contains an enumerator value of type
        /// <see cref="STORAGE_IDENTIFIER_TYPE"/> that indicates the identifier
        /// type.
        /// </summary>
        [FieldOffset(4)]
        public STORAGE_IDENTIFIER_TYPE Type;
        
        /// <summary>
        /// Specifies the size in bytes of the identifier.
        /// </summary>
        [FieldOffset(8)]
        public USHORT IdentifierSize;
        
        /// <summary>
        /// Specifies the offset in bytes from the current descriptor to the
        /// next descriptor.
        /// </summary>
        [FieldOffset(10)]
        public USHORT NextOffset;
        
        /// <summary>
        /// Contains an enumerator value of type
        /// <see cref="STORAGE_ASSOCIATION_TYPE"/> that indicates whether the
        /// descriptor identifies a device or a port.
        /// </summary>
        [FieldOffset(12)]
        public STORAGE_ASSOCIATION_TYPE Association;

        /// <summary>
        /// Contains the identifier associated with this descriptor.
        /// </summary>
        [FieldOffset(16)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public UCHAR[] Identifier;
    }
}