using Celeste.DataStructures;
using Celeste.Objects;
using UnityEditor;

namespace CelesteEditor.DataStructures
{
    public static class IIndexableItemsExtensions
    {
        public static bool SupportsGuids<T>()
        {
            return typeof(IIntGuid).IsAssignableFrom(typeof(T)) || typeof(IGuid).IsAssignableFrom(typeof(T));
        }
        
        public static bool SupportsGuids<T>(this IIndexableItems<T> indexableItems)
        {
            return SupportsGuids<T>();
        }

        public static void TrySyncGuids<T>(this IIndexableItems<T> indexableItems) where T : UnityEngine.Object
        {
            if (!SupportsGuids<T>())
            {
                return;
            }

            SyncGuids(indexableItems);
        }

        public static void SyncGuids<T>(this IIndexableItems<T> indexableItems) where T : UnityEngine.Object
        {
            if (!SupportsGuids<T>())
            {
                UnityEngine.Debug.LogAssertion($"Type {typeof(T).Name} does not implement the {nameof(IGuid)} or {nameof(IIntGuid)} interface.");
                return;
            }

            for (int i = 0, n = indexableItems.NumItems; i < n; i++)
            {
                T item = indexableItems.GetItem(i);
                
                if (item != null)
                {
                    if (item is IIntGuid intGuid)
                    {
                        intGuid.Guid = i + 1;     // 1 index the guids
                    }
                    else if (item is IGuid guid)
                    {
                        guid.Guid = string.IsNullOrEmpty(guid.Guid) ? System.Guid.NewGuid().ToString() : guid.Guid;
                    }

                    EditorUtility.SetDirty(item);
                }
            }
        }
    }
}