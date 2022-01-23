using Celeste.DataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Objects
{
    public class ArrayScriptableObject<T> : ScriptableObject, IIndexableItems<T>
    {
        #region Properties and Fields

        public int NumItems => items.Length;
        public IEnumerable<T> Items => items;

        [SerializeField] private T[] items;

        #endregion

        public T GetItem(int index)
        {
#if NULL_CHECKS
            if (items == null)
            {
                return default;
            }
#endif

#if INDEX_CHECKS
            if (index < 0 || index >= NumItems)
            {
                return default;
            }
#endif
            return items.Get(index);
        }

        public T FindItem(Predicate<T> predicate)
        {
#if NULL_CHECKS
            if (items == null)
            {
                return default;
            }
#endif
            return Array.Find(items, predicate);
        }
    }
}
