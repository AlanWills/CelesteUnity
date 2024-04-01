using Celeste.OdinSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Objects
{
    public class DictionaryScriptableObject<TKey, TValue> : SerializedScriptableObject, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        #region Change Block

        private struct ChangeBlock : IDisposable
        {
            private DictionaryScriptableObject<TKey, TValue> dictionarySO;

            public ChangeBlock(DictionaryScriptableObject<TKey, TValue> dictionarySO)
            {
                this.dictionarySO = dictionarySO;
                dictionarySO.OnPreModify();
            }

            public void Dispose()
            {
                dictionarySO.OnPostModify();
            }
        }

        #endregion

        #region Properties and Fields

        public int NumItems { get { return ItemsImpl.Count; } }
        public IReadOnlyDictionary<TKey, TValue> Items => ItemsImpl;

        private Dictionary<TKey, TValue> ItemsImpl => runtimeModifiedItems != null ? runtimeModifiedItems : items;

        [SerializeField] private Dictionary<TKey, TValue> items = new Dictionary<TKey, TValue>();

        [NonSerialized] private Dictionary<TKey, TValue> runtimeModifiedItems;

        #endregion

        public bool HasItem(TKey key)
        {
            return ItemsImpl.ContainsKey(key);
        }

        public TValue GetItem(TKey key)
        {
            return ItemsImpl.TryGetValue(key, out TValue value) ? value : default;
        }

        public bool TryFindItem(TKey key, out TValue value)
        {
            return ItemsImpl.TryGetValue(key, out value);
        }

        public void AddItem(TKey key, TValue value)
        {
            using (new ChangeBlock(this))
            {
#if KEY_CHECKS
                if (ItemsImpl.ContainsKey(key))
                {
                    UnityEngine.Debug.LogAssertion($"{name} already contains key {key}.");
                    return;
                }
#endif
                ItemsImpl.Add(key, value);
            }
        }

        public bool RemoveItem(TKey key)
        {
            using (new ChangeBlock(this))
            {
                return ItemsImpl.Remove(key);
            }
        }

        public void SetItems(IReadOnlyDictionary<TKey, TValue> newItems)
        {
            using (new ChangeBlock(this))
            {
                ItemsImpl.Clear();
                
                foreach (var item in newItems)
                {
                    ItemsImpl.Add(item.Key, item.Value);
                }
            }
        }

        public void Clear()
        {
            using (new ChangeBlock(this))
            {
                ItemsImpl.Clear();
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)ItemsImpl).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)ItemsImpl).GetEnumerator();
        }

        private void OnPreModify()
        {
            if (Application.isPlaying && runtimeModifiedItems == null)
            {
                // We've wanted to modify items for the first time at runtime so we create a copy of our serialized dictionary
                // to prevent any runtime changes affecting what will be serialized and saved
                runtimeModifiedItems = items != null ? new Dictionary<TKey, TValue>(items) : new Dictionary<TKey, TValue>();
            }
        }

        private void OnPostModify()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
        }
    }
}
