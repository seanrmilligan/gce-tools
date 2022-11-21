using System.Runtime.InteropServices;
using System.Text;

namespace Google.Cloud.Storage.Models.Extensions;

public static class ByteArrayExtensions
{
    private static char[] _hex = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
    
    public static string ToAsciiString(this byte[] array, uint offset)
    {
        return Encoding.ASCII.GetString(array
            .Skip((int)offset)
            .TakeWhile(b => b != 0)
            .ToArray());
    }

    public static string ToHexString(this byte[] array, string separator = "")
    {
        StringBuilder sb = new();
        
        foreach (byte b in array)
        {
            // logical right shift not available in project language version
            int upper = (b >> 4) & 0b1111;
            int lower = b & 0b1111;
            sb.Append($"{_hex[upper]}{_hex[lower]}{separator}");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Converts an array of bytes to an array of ints, using Big Endian
    /// format.
    /// </summary>
    /// <param name="array">The array of bytes to convert to ints.</param>
    /// <returns>An array of ints constructed in Big Endian format.</returns>
    /// <exception cref="ArgumentException">
    /// If the array length is not a multiple of sizeof(int32).
    /// </exception>
    public static int[] ToInt32ArrayBigEndian(this byte[] array)
    {
        const int intSize = 4;

        if (array.Length == 0)
        {
            return Array.Empty<int>();
        }
        
        if (array.Length % intSize != 0)
        {
            throw new ArgumentException($"Array length ({array.Length} bytes) must be a multiple of target data type size ({intSize} bytes).");
        }

        int[] result = new int[array.Length / intSize];

        for (int i = 0; i < result.Length; i++)
        {
            byte upper = array[i*intSize];
            byte middleHigh = array[i*intSize + 1];
            byte middleLow = array[i*intSize + 2];
            byte lower = array[i*intSize + 3];

            result[i] = (upper << 24) +
                        (middleHigh << 16) +
                        (middleLow << 8) +
                        lower;
        }

        return result;
    }
    
    /// <summary>
    /// Converts an array of bytes to an array of ints, using Little Endian
    /// format.
    /// </summary>
    /// <param name="array">The array of bytes to convert to ints.</param>
    /// <returns>An array of ints constructed in Little Endian format.</returns>
    /// <exception cref="ArgumentException">
    /// If the array length is not a multiple of sizeof(int32).
    /// </exception>
    public static int[] ToInt32Array(this byte[] array)
    {
        const int intSize = 4;

        if (array.Length == 0)
        {
            return Array.Empty<int>();
        }
        
        if (array.Length % intSize != 0)
        {
            throw new ArgumentException($"Array length ({array.Length} bytes) must be a multiple of target data type size ({intSize} bytes).");
        }

        int[] result = new int[array.Length / intSize];

        for (int i = 0; i < result.Length; i++)
        {
            byte upper = array[i*intSize + 3];
            byte middleHigh = array[i*intSize + 2];
            byte middleLow = array[i*intSize + 1];
            byte lower = array[i*intSize];

            result[i] = (upper << 24) +
                        (middleHigh << 16) +
                        (middleLow << 8) +
                        lower;
        }

        return result;
    }

    public static T ToStruct<T>(this byte[] array)
    {
        int bufferSize = Marshal.SizeOf(typeof(T));
        IntPtr buffer = Marshal.AllocHGlobal(bufferSize);
        
        Marshal.Copy(
            source: array,
            startIndex: 0,
            destination: buffer,
            length: bufferSize);

        return Marshal.PtrToStructure<T>(buffer)!;
    }
}