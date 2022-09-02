using System.Runtime.InteropServices;
using System.Text;

namespace Google.Cloud.Storage.Extensions
{
    public static class StructExtensions
    {
        private const int PageSize = 4096;
        private static char[] _hex = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        /// <exception cref="InternalBufferOverflowException"></exception>
        public static byte[] ToBytes<T>(this T strct) where T : struct
        {
            int size = Marshal.SizeOf(strct);
            byte[] arr = new byte[size];
  
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(strct, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }

        public static string ToHexString<T>(this T strct) where T : struct
        {
            StringBuilder sb = new();
            
            
            foreach (byte b in strct.ToBytes())
            {
                // logical right shift not available in project language version
                int upper = (b >> 4) & 0b1111;
                int lower = b & 0b1111;
                sb.Append($"{_hex[upper]}{_hex[lower]} ");
            }

            return sb.ToString();
        }
    }
}