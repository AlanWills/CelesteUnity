using System;
using System.Collections;
using System.Collections.ObjectModel;
using Unity.Collections;
using UnityEngine;
using Random = System.Random;

namespace Celeste.DataStructures
{
    public static class ArrayExtensions
    {
        public static bool Exists<T>(this T[] array, Predicate<T> predicate)
        {
#if NULL_CHECKS
            if (array == null)
            {
                Debug.LogAssertion($"Null array.");
                return default;
            }
#endif
            return Array.Exists(array, predicate);
        }

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

        public static int FindIndex<T>(this T[] array, Predicate<T> predicate)
        {
#if NULL_CHECKS
            if (array == null)
            {
                Debug.LogAssertion($"Null array.");
                return default;
            }
#endif
            return Array.FindIndex(array, predicate);
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

        public static void ResizeAndCopyFrom<T>(ref T[] destinationArray, T[] sourceArray)
        {
#if NULL_CHECKS
            if (sourceArray == null)
            {
                Array.Resize(ref destinationArray, 0);
                Debug.LogAssertion($"Null source array.");
                return;
            }
#endif
            if (destinationArray == null)
            {
                destinationArray = new T[sourceArray.Length];
            }
            else if (destinationArray.Length != sourceArray.Length)
            {
                Array.Resize(ref destinationArray, sourceArray.Length);
            }

            Array.Copy(sourceArray, destinationArray, sourceArray.Length);
        }

        public static ReadOnlyCollection<T> ToReadOnly<T>(this T[] array)
        {
            return new ReadOnlyCollection<T>(array);
        }

        public static T GetRandomItem<T>(this NativeArray<T>.ReadOnly array) where T : struct
        {
#if NULL_CHECKS
            if (array == null)
            {
                Debug.LogAssertion($"Null source array.");
                return default;
            }
#endif
            int numItems = array.Length;
            int randomIndex = UnityEngine.Random.Range(0, numItems);
            return array[randomIndex];
        }
    }
}