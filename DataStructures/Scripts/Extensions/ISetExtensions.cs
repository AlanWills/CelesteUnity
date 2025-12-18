using System.Collections.Generic;

namespace Celeste.DataStructures
{
    public static class ISetExtensions
    {
        public static void AddRange<T>(this ISet<T> set, ISet<T> items)
        {
            foreach (T item in items)
            {
                set.Add(item);
            }
        }
    }
}