using Celeste.DataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Celeste.Objects
{
    public class ListScriptableObject<T> : ScriptableObject, IIndexableItems<T>, IEnumerable<T>
    {
        #region Properties and Fields

        public int NumItems { get { return ItemsImpl.Count; } }
        public IReadOnlyList<T> Items => new ReadOnlyCollection<T>(ItemsImpl);

        private List<T> ItemsImpl => runtimeModifiedItems != null ? runtimeModifiedItems : items;

        [SerializeField] private List<T> items = new List<T>();

        [NonSerialized] private List<T> runtimeModifiedItems;

        #endregion

        public T GetItem(int index)
        {
#if INDEX_CHECKS
            if (index < 0 || index >= NumItems)
            {
                return default;
            }
#endif
            return ItemsImpl.Get(index);
        }

        public T FindItem(Predicate<T> predicate)
        {
            return ItemsImpl.Find(predicate);
        }

        public void AddItem(T item)
        {
            if (Application.isPlaying && runtimeModifiedItems == null)
            {
                // We've wanted to modify items for the first time at runtime so we create a copy of our serialized list
                // to prevent any runtime changes affecting what will be serialized and saved
                runtimeModifiedItems = new List<T>(items);
            }

            ItemsImpl.Add(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)items).GetEnumerator();
        }
    }
}
