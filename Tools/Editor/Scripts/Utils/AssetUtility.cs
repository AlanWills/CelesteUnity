using CelesteEditor.Tools.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Tools
{
    public static class AssetUtility
    {
        public static void AddObjectToAsset(Object objectToAdd, Object assetObject)
        {
            AssetDatabase.AddObjectToAsset(objectToAdd, assetObject);
            EditorUtility.SetDirty(assetObject);
        }

		public static void CreateAsset<T>(T asset, string path) where T : ScriptableObject
		{
            if (string.IsNullOrEmpty(asset.name))
            {
                asset.name = typeof(T).Name;
            }

			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{path}/{asset.name}.asset");

			AssetDatabase.CreateAsset(asset, assetPathAndName);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			SelectAsset(asset);
		}

        public static void SelectAsset(Object o)
        {
            Selection.activeObject = o;
            EditorUtility.FocusProjectWindow();
        }

		public static void CreateFolder(string parent, string folderName)
        {
			parent = parent.EndsWith("/") ? parent.Substring(0, parent.Length - 1) : parent;
			folderName = folderName.EndsWith("/") ? folderName.Substring(0, folderName.Length - 1) : folderName;
			AssetDatabase.CreateFolder(parent, folderName);
        }

		public static void ApplyHideFlags(Object obj, HideFlags hideFlags)
        {
            foreach (Object o in AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(obj)))
            {
                if (o != null && !AssetDatabase.IsMainAsset(o))
                {
                    o.hideFlags = hideFlags;
                    EditorUtility.SetDirty(o);
                    EditorUtility.SetDirty(obj);
                }
            }

            AssetDatabase.SaveAssets();
        }

        public static string GetSelectionObjectPath()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            return path;
        }

        public static string GetAssetFolderPath(Object obj)
        {
            string assetPath = AssetDatabase.GetAssetPath(obj);
            int indexOfSlash = assetPath.LastIndexOf('/');
            return indexOfSlash > 0 ? assetPath.Substring(0, indexOfSlash) : assetPath;
        }

        private static void FindAssetsImpl<T>(this Object target, string propertyName, string targetFolder) where T : ScriptableObject
        {
            SerializedObject serializedObject = new SerializedObject(target);
            serializedObject.Update();

            SerializedProperty objectsProperty = serializedObject.FindProperty(propertyName);

            bool dirty = FindAssets<T>(objectsProperty, targetFolder);

            serializedObject.ApplyModifiedProperties();

            if (dirty)
            {
                EditorUtility.SetDirty(target);
            }
        }

        public static bool FindAssets<T>(this SerializedProperty itemsProperty) where T : ScriptableObject
        {
            return FindAssets<T>(itemsProperty, "");
        }

        public static bool FindAssets<T>(this SerializedProperty itemsProperty, string targetFolder) where T : ScriptableObject
        {
            bool dirty = false;
            string[] objectGuids = new string[0];

            if (!string.IsNullOrEmpty(targetFolder))
            {
                objectGuids = AssetDatabase.FindAssets("t:" + typeof(T).Name, new string[] { targetFolder });
            }
            else
            {
                objectGuids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            }

            if (itemsProperty.arraySize != objectGuids.Length)
            {
                itemsProperty.arraySize = objectGuids.Length;
            }

            for (int i = 0; i < objectGuids.Length; ++i)
            {
                string objectPath = AssetDatabase.GUIDToAssetPath(objectGuids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(objectPath);

                SerializedProperty arrayElement = itemsProperty.GetArrayElementAtIndex(i);

                if (asset != arrayElement.objectReferenceValue)
                {
                    arrayElement.objectReferenceValue = asset;
                    dirty = true;
                }
            }

            return dirty;
        }

        public static void FindAssets<T>(this Object target, string propertyName) where T : ScriptableObject
        {
            FindAssetsImpl<T>(target, propertyName, "");
        }

        public static void FindAssets<T>(this Object target, string propertyName, string subDirectoryName) where T : ScriptableObject
        {
            string targetFolder = GetAssetFolderPath(target);
            if (!string.IsNullOrEmpty(subDirectoryName))
            {
                targetFolder = string.Format("{0}/{1}", targetFolder, subDirectoryName);
                targetFolder = targetFolder.EndsWith("/") ? targetFolder.Substring(0, targetFolder.Length - 1) : targetFolder;
            }

            FindAssetsImpl<T>(target, propertyName, targetFolder);
        }

        public static string[] FindAssets<T>(string name, string directory) where T : Object
        {
            if (!string.IsNullOrEmpty(directory))
            {
                string assetSearchString = new AssetDatabaseSearchBuilder().
                    WithAssetName(name).
                    WithType<T>().
                    Build();
                return AssetDatabase.FindAssets(assetSearchString, new string[] { directory });
            }
            else
            {
                return FindAssets<T>(name);
            }
        }

        public static string[] FindAssets<T>(string name) where T : Object
        {
            string assetSearchString = new AssetDatabaseSearchBuilder().
                       WithAssetName(name).
                       WithType<T>().
                       Build();
            return AssetDatabase.FindAssets(assetSearchString);
        }
    }
}
