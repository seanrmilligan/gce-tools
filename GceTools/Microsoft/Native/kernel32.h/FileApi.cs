using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Native.kernel32.h
{
    public class FileApi
    {
        // https://www.pinvoke.net/default.aspx/kernel32.createfile
        // https://codereview.stackexchange.com/q/23264
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern SafeFileHandle CreateFile(
            String lpFileName,
            UInt32 dwDesiredAccess,
            UInt32 dwShareMode,
            IntPtr lpSecurityAttributes,
            UInt32 dwCreationDisposition,
            UInt32 dwFlagsAndAttributes,
            IntPtr hTemplateFile
        );
    }
}
