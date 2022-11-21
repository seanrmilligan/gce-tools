namespace Google.Cloud.Storage.Models.Extensions
{
    public static class IEnumerableExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> enumerable, T obj)
        {
            int i = 0;

            foreach (T element in enumerable)
            {
                if (Equals(element, obj))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }
        
        public static bool None<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }
    }
}