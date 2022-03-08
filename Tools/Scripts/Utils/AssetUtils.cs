using UnityEngine;

namespace Celeste.Tools
{
#if UNITY_EDITOR
    public partial class EditorOnly
    {
        public static void AddObjectToMainAsset(Object objectToAdd, Object assetObject)
        {
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(assetObject);
            UnityEditor.AssetDatabase.AddObjectToAsset(objectToAdd, assetPath);
            UnityEditor.EditorUtility.SetDirty(assetObject);
        }
    }
#endif
}
