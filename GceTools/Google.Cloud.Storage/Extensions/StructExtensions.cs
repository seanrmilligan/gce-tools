using System.Runtime.InteropServices;
using System.Text;

namespace Google.Cloud.Storage.Extensions
{
    public static class StructExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// We have to know ahead of time the size of the struct we wish to
        /// marshal. We declare an arbitrary buffer size here and consider it
        /// sufficient.
        /// Taken from https://stackoverflow.com/questions/3278827/how-to-convert-a-structure-to-a-byte-array-in-c
        /// </remarks>
        /// <returns></returns>
        /// <exception cref="InternalBufferOverflowException"></exception>
        public static byte[] ToBytes<T>(this T strct) where T : struct
        {
            const int bufferSize = 4096*2;
            int size = Marshal.SizeOf(strct);
            byte[] arr = new byte[bufferSize];
            
            if (size > bufferSize)
            {
                throw new InternalBufferOverflowException(message:
                    "Size of struct exceeds buffer space allocated for it.");
            }
  
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(strct, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        public static string ToHexString<T>(this T strct) where T : struct
        {
            StringBuilder sb = new StringBuilder();
            char[] hex = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            
            foreach (byte b in strct.ToBytes())
            {
                // logical right shift not available in project language version
                int upper = (b >> 4) & 0b1111;
                int lower = b & 0b1111;
                sb.Append($"{hex[upper]}{hex[lower]} ");
            }

            return sb.ToString();
        }
    }
}