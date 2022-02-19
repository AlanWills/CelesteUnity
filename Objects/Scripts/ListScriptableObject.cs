using Celeste.DataStructures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Celeste.Objects
{
    public class ListScriptableObject<T> : ScriptableObject, IIndexableItems<T>
    {
        #region Properties and Fields

        public int NumItems { get { return items.Count; } }
        public ReadOnlyCollection<T> Items => new ReadOnlyCollection<T>(items);

        [SerializeField] private List<T> items = new List<T>();

        #endregion

        public T GetItem(int index)
        {
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
            return items.Find(predicate);
        }

        public void AddItem(T item)
        {
            items.Add(item);
        }
    }
}
