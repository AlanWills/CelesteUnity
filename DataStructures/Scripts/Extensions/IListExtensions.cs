using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DataStructures
{
    public static class IListExtensions
    {
        public static T Get<T>(this IReadOnlyList<T> list, int index)
        {
#if INDEX_CHECKS
            if (index < 0 || list.Count <= index)
            {
                Debug.LogAssertion($"Invalid index {index} into list.");
                return default;
            }
#endif
            return list[index];
        }

        public static T Get<T>(this IReadOnlyList<T> list, uint index)
        {
#if INDEX_CHECKS
            if (list.Count <= index)
            {
                Debug.LogAssertion($"Invalid index {index} into list.");
                return default;
            }
#endif
            return list[(int)index];
        }

        public static int GetRandomIndex<T>(this IReadOnlyList<T> list)
        {
            return Random.Range(0, list.Count);
        }

        public static Dictionary<string, T> ToNameLookup<T>(this IList<T> list) where T : Object
        {
            Dictionary<string, T> lookup = new Dictionary<string, T>(list.Count);

            for (int i = 0, n = list.Count; i < n; ++i)
            {
                T value = list[i];

#if KEY_CHECKS
                if (lookup.ContainsKey(value.name))
                {
                    Debug.LogAssertionFormat("Lookup already contains {0}", value.name);
                    continue;
                }
#endif
                lookup.Add(value.name, value);
            }

            return lookup;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                T temp = list[i];
                int randomIndex = Random.Range(i, list.Count);
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }

        public static bool Exists<T>(this IReadOnlyList<T> list, System.Predicate<T> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Contains<T>(this IReadOnlyList<T> list, T value)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (value.Equals(list[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public static void AssignFrom<T>(this List<T> list, IEnumerable<T> source)
        {
            list.Clear();
            list.AddRange(source);
        }
    }
}