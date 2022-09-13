using System.Text;

namespace Google.Cloud.Storage.Extensions;

public static class ByteArrayExtensions
{
    public static string GetAsciiString(this byte[] array, uint offset)
    {
        return Encoding.ASCII.GetString(array
            .Skip((int)offset)
            .TakeWhile(b => b != 0)
            .ToArray());
    }
}