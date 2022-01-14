using System.Runtime.InteropServices;

using ULONG = System.UInt32;

namespace Google.Cloud.Storage.Windows.winioctl
{
    [StructLayout(LayoutKind.Sequential)]
    public struct STORAGE_PROTOCOL_SPECIFIC_DATA
    {
        public STORAGE_PROTOCOL_TYPE ProtocolType;
        public ULONG   DataType;                 

        public ULONG   ProtocolDataRequestValue;
        public ULONG   ProtocolDataRequestSubValue;

        public ULONG   ProtocolDataOffset;         
        public ULONG   ProtocolDataLength;

        public ULONG   FixedProtocolReturnData;
        public ULONG Reserved; // Reserved[3];
    }
}