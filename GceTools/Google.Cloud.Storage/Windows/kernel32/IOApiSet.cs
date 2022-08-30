using System.Runtime.InteropServices;
using Google.Cloud.Storage.Windows.winioctl;
using Microsoft.Win32.SafeHandles;

using LPSECURITY_ATTRIBUTES = System.IntPtr;
using LPOVERLAPPED = System.IntPtr;
using HANDLE = System.IntPtr;
using DWORD = System.UInt32;
using LPCTSTR = System.String;

namespace Google.Cloud.Storage.Windows.kernel32
{
    public class IOApiSet
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            DWORD dwIoControlCode,
            ref STORAGE_PROPERTY_QUERY lpInBuffer,
            DWORD nInBufferSize,
            out STORAGE_ADAPTER_DESCRIPTOR lpOutBuffer,
            int nOutBufferSize,
            ref DWORD lpBytesReturned,
            LPOVERLAPPED lpOverlapped
        );
        
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            DWORD dwIoControlCode,
            ref STORAGE_PROPERTY_QUERY lpInBuffer,
            DWORD nInBufferSize,
            out STORAGE_DEVICE_DESCRIPTOR lpOutBuffer,
            int nOutBufferSize,
            ref DWORD lpBytesReturned,
            LPOVERLAPPED lpOverlapped
        );

        // https://www.pinvoke.net/default.aspx/kernel32.deviceiocontrol
        // https://codereview.stackexchange.com/q/23264
        // https://stackoverflow.com/a/17354960/1230197
        // https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.marshal
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            DWORD dwIoControlCode,
            ref STORAGE_PROPERTY_QUERY lpInBuffer,
            DWORD nInBufferSize,
            out STORAGE_DEVICE_ID_DESCRIPTOR lpOutBuffer,
            int nOutBufferSize,
            ref DWORD lpBytesReturned,
            LPOVERLAPPED lpOverlapped
        );
    
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            DWORD dwIoControlCode,
            System.IntPtr lpInBuffer,
            DWORD nInBufferSize,
            System.IntPtr  lpOutBuffer,
            int nOutBufferSize,
            ref DWORD lpBytesReturned,
            LPOVERLAPPED lpOverlapped
        );
    }
}