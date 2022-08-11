using System.Collections.Generic;
using System.Linq;

namespace Google.Cloud.Storage.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool None<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }
    }
}