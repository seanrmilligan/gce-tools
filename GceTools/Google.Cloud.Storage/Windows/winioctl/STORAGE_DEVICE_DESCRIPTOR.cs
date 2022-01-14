using System.Runtime.InteropServices;
using BOOLEAN = System.Boolean;
using BYTE = System.Byte;
using DWORD = System.UInt32;
using ULONG = System.UInt32;

namespace Google.Cloud.Storage.Windows.winioctl
{
    [StructLayout(LayoutKind.Sequential)]
    public struct STORAGE_DEVICE_DESCRIPTOR
    {
        public DWORD Version;
        public DWORD Size;
        public BYTE DeviceType;
        public BYTE DeviceTypeModifier;
        public BOOLEAN RemovableMedia;
        public BOOLEAN CommandQueueing;
        public DWORD VendorIdOffset;
        public DWORD ProductIdOffset;
        public DWORD ProductRevisionOffset;
        public DWORD SerialNumberOffset;
        public STORAGE_BUS_TYPE BusType;
        public DWORD RawPropertiesLength;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public BYTE[] RawDeviceProperties; // array of size 1
    }
}