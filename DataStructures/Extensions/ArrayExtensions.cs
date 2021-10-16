using System;
using System.Collections;
using UnityEngine;

namespace Celeste.DataStructures
{
    public static class ArrayExtensions
    {
        public static T Find<T>(this T[] array, Predicate<T> predicate)
        {
#if NULL_CHECKS
            if (array == null)
            {
                Debug.LogAssertion($"Null array.");
                return default;
            }
#endif
            return Array.Find(array, predicate);
        }
        public static T Get<T>(this T[] array, int index)
        {
#if NULL_CHECKS
            if (array == null)
            {
                Debug.LogAssertion($"Null array.");
                return default;
            }
#endif
#if INDEX_CHECKS
            if (index < 0 || array.Length <= index)
            {
                Debug.LogAssertion($"Invalid index {index} into array.");
                return default;
            }
#endif

            return array[index];
        }
    }
}