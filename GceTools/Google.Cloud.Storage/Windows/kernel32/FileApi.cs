using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

using LPSECURITY_ATTRIBUTES = System.IntPtr;
using LPOVERLAPPED = System.IntPtr;
using HANDLE = System.IntPtr;
using DWORD = System.UInt32;
using LPCTSTR = System.String;

namespace Google.Cloud.Storage.Windows.kernel32
{
    public class FileApi
    {
        // https://www.pinvoke.net/default.aspx/kernel32.createfile
        // https://codereview.stackexchange.com/q/23264
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern SafeFileHandle CreateFile(
            LPCTSTR lpFileName,
            DWORD dwDesiredAccess,
            DWORD dwShareMode,
            LPSECURITY_ATTRIBUTES lpSecurityAttributes,
            DWORD dwCreationDisposition,
            DWORD dwFlagsAndAttributes,
            HANDLE hTemplateFile
        );
    }
}
