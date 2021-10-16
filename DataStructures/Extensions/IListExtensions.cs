using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DataStructures
{
    public static class IListExtensions
    {
        public static T Get<T>(this IList<T> list, int index)
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

        public static T Get<T>(this IList<T> list, uint index)
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
    }
}