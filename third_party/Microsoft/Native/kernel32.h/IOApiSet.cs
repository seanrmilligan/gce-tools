using System;
using System.Runtime.InteropServices;
using Microsoft.Native.winioctl.h;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Native.kernel32.h
{
    public class IOApiSet
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            UInt32 dwIoControlCode,
            ref STORAGE_PROPERTY_QUERY lpInBuffer,
            UInt32 nInBufferSize,
            out STORAGE_ADAPTER_DESCRIPTOR lpOutBuffer,
            int nOutBufferSize,
            ref UInt32 lpBytesReturned,
            IntPtr lpOverlapped
        );
        
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            UInt32 dwIoControlCode,
            ref STORAGE_PROPERTY_QUERY lpInBuffer,
            UInt32 nInBufferSize,
            out STORAGE_DEVICE_DESCRIPTOR lpOutBuffer,
            int nOutBufferSize,
            ref UInt32 lpBytesReturned,
            IntPtr lpOverlapped
        );

        // https://www.pinvoke.net/default.aspx/kernel32.deviceiocontrol
        // https://codereview.stackexchange.com/q/23264
        // https://stackoverflow.com/a/17354960/1230197
        // https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.marshal
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            UInt32 dwIoControlCode,
            ref STORAGE_PROPERTY_QUERY lpInBuffer,
            UInt32 nInBufferSize,
            out STORAGE_DEVICE_ID_DESCRIPTOR lpOutBuffer,
            int nOutBufferSize,
            ref UInt32 lpBytesReturned,
            IntPtr lpOverlapped
        );
    
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            UInt32 dwIoControlCode,
            System.IntPtr lpInBuffer,
            UInt32 nInBufferSize,
            System.IntPtr  lpOutBuffer,
            int nOutBufferSize,
            ref UInt32 lpBytesReturned,
            IntPtr lpOverlapped
        );
    }
}