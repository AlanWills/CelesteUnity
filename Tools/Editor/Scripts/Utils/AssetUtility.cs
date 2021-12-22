using CelesteEditor.Tools.Utils;
using System.IO;
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

		public static void CreateAsset<T>(T asset, string parentFolder) where T : ScriptableObject
		{
            if (string.IsNullOrEmpty(asset.name))
            {
                asset.name = typeof(T).Name;
            }

			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{parentFolder}/{asset.name}.asset");

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

        /// <summary>
        /// Creates a folder relative to the project directory.  
        /// As such, to create an object in Assets/ the path must start with Assets/.
        /// </summary>
        /// <param name="newFolder"></param>
        public static void CreateFolder(string newFolder)
        {
            int indexOfLastDelimiter = newFolder.LastIndexOf('/');
            CreateFolder(newFolder.Substring(0, indexOfLastDelimiter), newFolder.Substring(indexOfLastDelimiter + 1));
        }

        public static void CreateFolder(string parent, string folderName)
        {
			parent = parent.EndsWith("/") ? parent.Substring(0, parent.Length - 1) : parent;
			folderName = folderName.EndsWith("/") ? folderName.Substring(0, folderName.Length - 1) : folderName;

            if (!AssetDatabase.IsValidFolder(Path.Combine(parent, folderName)))
            {
                AssetDatabase.CreateFolder(parent, folderName);
            }
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

        public static T FindAsset<T>(string name) where T : Object
        {
            return FindAsset<T>(name, string.Empty);
        }

        public static T FindAsset<T>(string name, string directory) where T : Object
        {
            string[] assetGuids = FindAssets<T>(name, directory);
            if (assetGuids != null && assetGuids.Length == 1)
            {
                return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(assetGuids[0]));
            }

            Debug.LogAssertion($"Could not find exactly one asset of type '{typeof(T).Name}' and name '{name}'.");
            return default;
        }
    }
}
