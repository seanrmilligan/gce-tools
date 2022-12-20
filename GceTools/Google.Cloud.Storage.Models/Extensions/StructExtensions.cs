/* Copyright 2022 Google LLC
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     https://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System.Runtime.InteropServices;
using System.Text;

namespace Google.Cloud.Storage.Models.Extensions
{
    public static class StructExtensions
    {
        private static char[] _hex = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        public static string ToAsciiString<T>(this T strct) where T : struct
        {
            return Encoding.ASCII.GetString(strct.ToBytes());
        }
        
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