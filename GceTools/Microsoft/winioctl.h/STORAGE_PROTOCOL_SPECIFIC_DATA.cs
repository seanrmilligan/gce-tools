// Copyright (c) Microsoft Corporation. All rights reserved.
// https://docs.microsoft.com/en-us/windows/win32/api/winioctl/ns-winioctl-storage_protocol_specific_data

using System.Runtime.InteropServices;
using DWORD = System.UInt32;

// Adapted from winioctl.h
namespace Google.Cloud.Storage.Windows.winioctl
{
    /// <summary>
    /// Describes protocol-specific device data, provided in the input and
    /// output buffer of an IOCTL_STORAGE_QUERY_PROPERTY request.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct STORAGE_PROTOCOL_SPECIFIC_DATA
    {
        /// <summary>
        /// The protocol type. Values for this member are defined in the
        /// <see cref="STORAGE_PROTOCOL_TYPE"/> enumeration.
        /// </summary>
        public STORAGE_PROTOCOL_TYPE ProtocolType;
        
        /// <summary>
        /// The protocol data type. Data types are defined in the
        /// STORAGE_PROTOCOL_NVME_DATA_TYPE and STORAGE_PROTOCOL_ATA_DATA_TYPE
        /// enumerations.
        /// </summary>
        public DWORD DataType;
        
        /// <summary>
        /// The protocol data request value.
        /// </summary>
        public DWORD ProtocolDataRequestValue;
        
        /// <summary>
        /// The sub value of the protocol data request.
        /// </summary>
        public DWORD ProtocolDataRequestSubValue;
        
        /// <summary>
        /// The offset of the data buffer that is from the beginning of this
        /// structure. The typical value can be
        /// sizeof(STORAGE_PROTOCOL_SPECIFIC_DATA).
        /// </summary>
        public DWORD ProtocolDataOffset;
        
        /// <summary>
        /// The length of the protocol data.
        /// </summary>
        public DWORD ProtocolDataLength;
        
        /// <summary>
        /// The returned data.
        /// </summary>
        public DWORD FixedProtocolReturnData;
        
        /// <summary>
        /// 
        /// </summary>
        public DWORD ProtocolDataRequestSubValue2;
        
        /// <summary>
        /// 
        /// </summary>
        public DWORD ProtocolDataRequestSubValue3;
        
        /// <summary>
        /// 
        /// </summary>
        public DWORD ProtocolDataRequestSubValue4;

        public override string ToString()
        {
            return string.Join(System.Environment.NewLine, new[]
            {
               $"ProtocolType:                 {ProtocolType}",
               $"DataType:                     {(STORAGE_PROTOCOL_NVME_DATA_TYPE) DataType}",
               $"ProtocolDataRequestValue:     {ProtocolDataRequestValue}",
               $"ProtocolDataRequestSubValue:  {ProtocolDataRequestSubValue}",
               $"ProtocolDataOffset:           {ProtocolDataOffset}",
               $"ProtocolDataLength:           {ProtocolDataLength}",
               $"FixedProtocolReturnData:      {FixedProtocolReturnData}",
               $"ProtocolDataRequestSubValue2: {ProtocolDataRequestSubValue2}",
               $"ProtocolDataRequestSubValue3: {ProtocolDataRequestSubValue3}",
               $"ProtocolDataRequestSubValue4: {ProtocolDataRequestSubValue4}"
            });
        }
    }
}