using System.Diagnostics;
using UnityEngine;

namespace Celeste.Tools
{
    public partial class EditorOnly
    {
        [Conditional("UNITY_EDITOR")]
        public static void AddObjectToAsset(Object objectToAdd, Object assetObject)
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.AddObjectToAsset(objectToAdd, assetObject);
            UnityEditor.EditorUtility.SetDirty(assetObject);
            UnityEditor.AssetDatabase.SaveAssetIfDirty(assetObject);
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void RemoveObjectFromAsset(Object objectToRemove, bool destroyAsset)
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.RemoveObjectFromAsset(objectToRemove);

            if (destroyAsset)
            {
                Object.DestroyImmediate(objectToRemove, true);
            }
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void SetDirty(Object objectToSetDirty)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(objectToSetDirty);
#endif
        }
    }
}
