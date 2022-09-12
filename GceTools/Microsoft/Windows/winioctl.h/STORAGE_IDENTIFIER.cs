// Copyright (c) Microsoft Corporation. All rights reserved.

using System.Runtime.InteropServices;
using UCHAR = System.Byte;
using USHORT = System.UInt16;

namespace Microsoft.Windows.winioctl.h
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public UCHAR[] Identifier;
    }
}