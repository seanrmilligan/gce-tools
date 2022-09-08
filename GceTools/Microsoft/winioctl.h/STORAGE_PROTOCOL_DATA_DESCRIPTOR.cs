// Copyright (c) Microsoft Corporation. All rights reserved.

using System.Runtime.InteropServices;
using DWORD = System.UInt32;

// Adapted from winioctl.h
namespace Microsoft.winioctl.h
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct STORAGE_PROTOCOL_DATA_DESCRIPTOR
    {
        /// <summary>
        /// The version of this structure.
        /// </summary>
        public readonly DWORD Version;
        
        /// <summary>
        /// The total size of the descriptor, including the space for all
        /// protocol data.
        /// </summary>
        public readonly DWORD Size;
        
        /// <summary>
        /// The protocol-specific data, of type
        /// <see cref="STORAGE_PROTOCOL_SPECIFIC_DATA"/>.
        /// </summary>
        public readonly STORAGE_PROTOCOL_SPECIFIC_DATA ProtocolSpecificData;

        public override string ToString()
        {
            string indent = Environment.NewLine + "  ";
            return string.Join(Environment.NewLine, new[]
            {
                $"Version:               {Version}",
                $"Size:                  {Size}",
                $"ProtocolSpecificData:",
                $"  {ProtocolSpecificData.ToString().Replace(Environment.NewLine, indent)}"
            });
        }
    } 
}