using System.Diagnostics;
using UnityEngine;

namespace CelesteEditor.Tools
{
    public static class AssetDatabaseExtensions
    {
        public static bool FindAssets<T>(this UnityEditor.SerializedProperty itemsProperty) where T : Object
        {
            return FindAssets<T>(itemsProperty, string.Empty);
        }

        public static bool FindAssets<T>(this UnityEditor.SerializedProperty itemsProperty, string targetFolder) where T : Object
        {
            bool dirty = false;
            string[] objectGuids;

            if (!string.IsNullOrEmpty(targetFolder))
            {
                objectGuids = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name, new string[] { targetFolder });
            }
            else
            {
                objectGuids = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);
            }

            if (itemsProperty.arraySize != objectGuids.Length)
            {
                itemsProperty.arraySize = objectGuids.Length;
            }

            for (int i = 0; i < objectGuids.Length; ++i)
            {
                string objectPath = UnityEditor.AssetDatabase.GUIDToAssetPath(objectGuids[i]);
                T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(objectPath);

                UnityEditor.SerializedProperty arrayElement = itemsProperty.GetArrayElementAtIndex(i);

                if (asset != arrayElement.objectReferenceValue)
                {
                    arrayElement.objectReferenceValue = asset;
                    dirty = true;
                }
            }

            return dirty;
        }

        [Conditional("UNITY_EDITOR")]
        public static void FindAssets<T>(this Object target, string propertyName) where T : Object
        {
#if UNITY_EDITOR
            FindAssetsImpl<T>(target, propertyName, string.Empty);
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void FindAssets<T>(this Object target, string propertyName, string subDirectoryName) where T : Object
        {
            string targetFolder = Celeste.Tools.EditorOnly.GetAssetFolderPath(target);
            if (!string.IsNullOrEmpty(subDirectoryName))
            {
                targetFolder = $"{targetFolder}/{subDirectoryName}";
                targetFolder = Celeste.Tools.EditorOnly.StripTrailingSlash(targetFolder);
            }

            FindAssetsImpl<T>(target, propertyName, targetFolder);
        }

        [Conditional("UNITY_EDITOR")]
        private static void FindAssetsImpl<T>(this Object target, string propertyName, string targetFolder) where T : Object
        {
            UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(target);
            serializedObject.Update();

            UnityEditor.SerializedProperty objectsProperty = serializedObject.FindProperty(propertyName);

            bool dirty = FindAssets<T>(objectsProperty, targetFolder);

            serializedObject.ApplyModifiedProperties();

            if (dirty)
            {
                UnityEditor.EditorUtility.SetDirty(target);
            }
        }
    }
}