using CelesteEditor.Tools.Utils;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Tools
{
    public static class EditorOnly
    {
        #region Utils

        public static void EnsureNamed(Object asset)
        {
            if (string.IsNullOrEmpty(asset.name))
            {
                asset.name = asset.GetType().Name;
            }
        }

        public static string StripTrailingSlash(string path)
        {
            return path.EndsWith("/") ? path.Substring(0, path.Length - 1) : path;
        }

        public static string EnsureRelativeToAssets(string path)
        {
            if (!path.StartsWith("Assets"))
            {
                path = Path.Combine("Assets", path);
            }

            return StripTrailingSlash(EnsureDelimitersCorrect(path));
        }

        public static string EnsureDelimitersCorrect(string path)
        {
            return path.Replace('\\', '/');
        }

        #endregion

        public static void AddObjectToAsset(Object objectToAdd, Object assetObject)
        {
            AssetDatabase.AddObjectToAsset(objectToAdd, assetObject);
            EditorUtility.SetDirty(assetObject);
        }

        public static bool AssetExists(string name, string parentFolder)
        {
            parentFolder = EnsureRelativeToAssets(parentFolder);
            return AssetExists(Path.Combine(parentFolder, $"{name}.asset"));
        }

        public static bool AssetExists(string assetPath)
        {
            assetPath = EnsureRelativeToAssets(assetPath);
            return !string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(assetPath, AssetPathToGUIDOptions.OnlyExistingAssets));
        }

        public static void CreateAsset(Object asset, string assetPath)
        {
            int indexOfLastDelimiter = assetPath.LastIndexOf('/');
            string parentFolder = assetPath.Substring(0, indexOfLastDelimiter);
            
            CreateFolder(parentFolder);
            EnsureNamed(asset);

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(assetPath);
            AssetDatabase.CreateAsset(asset, assetPathAndName);
        }

        public static void CreateAssetInFolder(Object asset, string parentFolder)
        {
            EnsureNamed(asset);
            CreateAsset(asset, $"{parentFolder}/{asset.name}.asset");
        }

        public static void CreateAssetInFolderAndSave(Object asset, string parentFolder)
		{
            CreateAssetInFolder(asset, parentFolder);

			AssetDatabase.SaveAssetIfDirty(asset);
			AssetDatabase.Refresh();

			SelectAsset(asset);
		}

        public static void SelectAsset(Object o)
        {
            Selection.activeObject = o;
            EditorUtility.FocusProjectWindow();
        }

        /// <summary>
        /// Creates a folder relative to the Assets directory.  
        /// If the newFolder path does not already start with Assets/, it will be added.
        /// </summary>
        /// <param name="newFolder"></param>
        public static void CreateFolder(string newFolder)
        {
            if (string.IsNullOrEmpty(newFolder))
            {
                Debug.LogAssertion($"Cannot enter empty string into {nameof(CreateFolder)}.");
                return;
            }

            newFolder = EnsureRelativeToAssets(newFolder);

            int indexOfLastDelimiter =  newFolder.LastIndexOf('/');
            if (indexOfLastDelimiter > 0)
            {
                CreateFolder(newFolder.Substring(0, indexOfLastDelimiter), newFolder.Substring(indexOfLastDelimiter + 1));
            }
        }

        public static void CreateFolder(string parent, string folderName)
        {
            parent = EnsureRelativeToAssets(parent);

            // Ensure all folders are created in our hierarchy
            if (!AssetDatabase.IsValidFolder(parent))
            {
                CreateFolder(parent);
            }

            folderName = StripTrailingSlash(folderName);

            if (!AssetDatabase.IsValidFolder(Path.Combine(parent, folderName)))
            {
                string folderGuid = AssetDatabase.CreateFolder(parent, folderName);
                Debug.Assert(!string.IsNullOrEmpty(folderGuid), $"Failed to create folder {folderName} in parent folder {parent}.");

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
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

        public static void RemoveHideFlags(Object obj, HideFlags hideFlags)
        {
            foreach (Object o in AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(obj)))
            {
                if (o != null && !AssetDatabase.IsMainAsset(o))
                {
                    o.hideFlags &= ~hideFlags;
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

        private static void FindAssetsImpl<T>(this Object target, string propertyName, string targetFolder) where T : Object
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

        public static bool FindAssets<T>(this SerializedProperty itemsProperty) where T : Object
        {
            return FindAssets<T>(itemsProperty, "");
        }

        public static bool FindAssets<T>(this SerializedProperty itemsProperty, string targetFolder) where T : Object
        {
            bool dirty = false;
            string[] objectGuids;

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

        public static void FindAssets<T>(this Object target, string propertyName) where T : Object
        {
            FindAssetsImpl<T>(target, propertyName, "");
        }

        public static void FindAssets<T>(this Object target, string propertyName, string subDirectoryName) where T : Object
        {
            string targetFolder = GetAssetFolderPath(target);
            if (!string.IsNullOrEmpty(subDirectoryName))
            {
                targetFolder = $"{targetFolder}/{subDirectoryName}";
                targetFolder = StripTrailingSlash(targetFolder);
            }

            FindAssetsImpl<T>(target, propertyName, targetFolder);
        }

        public static List<T> FindAssets<T>(string name, string directory) where T : Object
        {
            if (!string.IsNullOrEmpty(directory))
            {
                string assetSearchString = new AssetDatabaseSearchBuilder().
                    WithAssetName(name).
                    WithType<T>().
                    Build();
                
                List<T> assets = new List<T>();
                foreach (string guid in AssetDatabase.FindAssets(assetSearchString, new string[] { directory }))
                {
                    T asset = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
                    assets.Add(asset);
                }

                return assets;
            }
            else
            {
                return FindAssets<T>(name);
            }
        }

        public static List<T> FindAssets<T>() where T : Object
        {
            string assetSearchString = new AssetDatabaseSearchBuilder().
                       WithType<T>().
                       Build();

            List<T> assets = new List<T>();
            foreach (string guid in AssetDatabase.FindAssets(assetSearchString))
            {
                T asset = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
                assets.Add(asset);
            }

            return assets;
        }

        public static List<T> FindAssets<T>(string name) where T : Object
        {
            string assetSearchString = new AssetDatabaseSearchBuilder().
                       WithAssetName(name).
                       WithType<T>().
                       Build();

            List<T> assets = new List<T>();
            foreach (string guid in AssetDatabase.FindAssets(assetSearchString))
            {
                T asset = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
                assets.Add(asset);
            }

            return assets;
        }

        public static T FindAsset<T>() where T : Object
        {
            return FindAsset<T>(string.Empty, string.Empty);
        }

        public static T FindAsset<T>(string name) where T : Object
        {
            return FindAsset<T>(name, string.Empty);
        }

        public static T FindAsset<T>(string name, string directory) where T : Object
        {
            List<T> assets = FindAssets<T>(name, directory);
            
            if (string.IsNullOrEmpty(name) && assets.Count == 1)
            {
                return assets[0];
            }

            for (int i = 0, n = assets.Count; i < n; ++i)
            {
                T asset = assets[i];
                if (string.CompareOrdinal(asset.name, name) == 0)
                {
                    return asset;
                }
            }

            Debug.LogAssertion($"Could not find exactly one asset of type '{typeof(T).Name}' and name '{name}'.  Found: {assets.Count}.");
            return default;
        }
    }
}
