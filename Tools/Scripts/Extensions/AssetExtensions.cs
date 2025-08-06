using Celeste.Tools.Utils;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace Celeste.Tools
{
    public static class EditorOnly
    {
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

        [Conditional("UNITY_EDITOR")]
        public static void AddObjectToAsset(this Object objectToAddToAsset, Object assetThatWillHoldObject)
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.AddObjectToAsset(objectToAddToAsset, assetThatWillHoldObject);
            UnityEditor.EditorUtility.SetDirty(assetThatWillHoldObject);
            UnityEditor.AssetDatabase.SaveAssetIfDirty(assetThatWillHoldObject);
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

        [Conditional("UNITY_EDITOR")]
        public static void SaveAsset(Object objectToSave)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(objectToSave);
            UnityEditor.AssetDatabase.SaveAssetIfDirty(objectToSave);
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        public static bool AssetExists(string name, string parentFolder)
        {
#if UNITY_EDITOR
            parentFolder = EnsureRelativeToAssets(parentFolder);
            return AssetExists(Path.Combine(parentFolder, $"{name}.asset"));
#else
            return false;
#endif
        }

        public static bool AssetExists(string assetPath)
        {
#if UNITY_EDITOR
            assetPath = EnsureRelativeToAssets(assetPath);
            return !string.IsNullOrEmpty(UnityEditor.AssetDatabase.AssetPathToGUID(assetPath, UnityEditor.AssetPathToGUIDOptions.OnlyExistingAssets));
#else
            return false;
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void CreateAsset(Object asset, string assetPath)
        {
#if UNITY_EDITOR
            int indexOfLastDelimiter = assetPath.LastIndexOf('/');
            string parentFolder = assetPath.Substring(0, indexOfLastDelimiter);

            CreateFolder(parentFolder);
            EnsureNamed(asset);

            string assetPathAndName = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(assetPath);
            UnityEditor.AssetDatabase.CreateAsset(asset, assetPathAndName);
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void CreateAssetInFolder(Object asset, string parentFolder)
        {
#if UNITY_EDITOR
            EnsureNamed(asset);
            CreateAsset(asset, $"{parentFolder}/{asset.name}.asset");
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void CreateAssetInFolderAndSave(Object asset, string parentFolder)
        {
#if UNITY_EDITOR
            CreateAssetInFolder(asset, parentFolder);

            UnityEditor.AssetDatabase.SaveAssetIfDirty(asset);
            UnityEditor.AssetDatabase.Refresh();

            SelectAsset(asset);
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void SelectAsset(Object o)
        {
#if UNITY_EDITOR
            UnityEditor.Selection.activeObject = o;
            UnityEditor.EditorUtility.FocusProjectWindow();
#endif
        }

        /// <summary>
        /// Creates a folder relative to the Assets directory.  
        /// If the newFolder path does not already start with Assets/, it will be added.
        /// </summary>
        /// <param name="newFolder"></param>
        [Conditional("UNITY_EDITOR")]
        public static void CreateFolder(string newFolder)
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(newFolder))
            {
                UnityEngine.Debug.LogAssertion($"Cannot enter empty string into {nameof(CreateFolder)}.");
                return;
            }

            newFolder = EnsureRelativeToAssets(newFolder);

            int indexOfLastDelimiter = newFolder.LastIndexOf('/');
            if (indexOfLastDelimiter > 0)
            {
                CreateFolder(newFolder.Substring(0, indexOfLastDelimiter), newFolder.Substring(indexOfLastDelimiter + 1));
            }
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void CreateFolder(string parent, string folderName)
        {
#if UNITY_EDITOR
            parent = EnsureRelativeToAssets(parent);

            // Ensure all folders are created in our hierarchy
            if (!UnityEditor.AssetDatabase.IsValidFolder(parent))
            {
                CreateFolder(parent);
            }

            folderName = StripTrailingSlash(folderName);

            if (!UnityEditor.AssetDatabase.IsValidFolder(Path.Combine(parent, folderName)))
            {
                string folderGuid = UnityEditor.AssetDatabase.CreateFolder(parent, folderName);
                UnityEngine.Debug.Assert(!string.IsNullOrEmpty(folderGuid), $"Failed to create folder {folderName} in parent folder {parent}.");

                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();
            }
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void ApplyHideFlags(this Object obj, HideFlags hideFlags)
        {
#if UNITY_EDITOR
            foreach (Object o in UnityEditor.AssetDatabase.LoadAllAssetsAtPath(UnityEditor.AssetDatabase.GetAssetPath(obj)))
            {
                if (o != null && !UnityEditor.AssetDatabase.IsMainAsset(o))
                {
                    o.hideFlags = hideFlags;
                    UnityEditor.EditorUtility.SetDirty(o);
                    UnityEditor.EditorUtility.SetDirty(obj);
                }
            }

            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void RemoveHideFlags(this Object obj, HideFlags hideFlags)
        {
#if UNITY_EDITOR
            foreach (Object o in UnityEditor.AssetDatabase.LoadAllAssetsAtPath(UnityEditor.AssetDatabase.GetAssetPath(obj)))
            {
                if (o != null && !UnityEditor.AssetDatabase.IsMainAsset(o))
                {
                    o.hideFlags &= ~hideFlags;
                    UnityEditor.EditorUtility.SetDirty(o);
                    UnityEditor.EditorUtility.SetDirty(obj);
                }
            }

            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }

        public static string GetSelectionObjectPath()
        {
#if UNITY_EDITOR
            string path = UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject);
            if (string.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != string.Empty)
            {
                path = path.Replace(Path.GetFileName(UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject)), string.Empty);
            }

            return path;
#else
            return string.Empty;
#endif
        }

        public static string GetAssetFolderPath(Object obj)
        {
#if UNITY_EDITOR
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(obj);
            int indexOfSlash = assetPath.LastIndexOf('/');
            return indexOfSlash > 0 ? assetPath.Substring(0, indexOfSlash) : assetPath;
#else
            return string.Empty;
#endif
        }

        public static List<T> FindAssets<T>(string name, string directory) where T : Object
        {
#if UNITY_EDITOR
            if (!string.IsNullOrEmpty(directory))
            {
                string assetSearchString = new AssetDatabaseSearchBuilder().
                    WithAssetName(name).
                    WithType<T>().
                    Build();

                List<T> assets = new List<T>();
                foreach (string guid in UnityEditor.AssetDatabase.FindAssets(assetSearchString, new string[] { directory }))
                {
                    T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));
                    assets.Add(asset);
                }

                return assets;
            }
            else
            {
                return FindAssets<T>(name);
            }
#else
            return new List<T>();
#endif
        }

        public static List<T> FindAssets<T>() where T : Object
        {
#if UNITY_EDITOR
            string assetSearchString = new AssetDatabaseSearchBuilder().
                       WithType<T>().
                       Build();

            List<T> assets = new List<T>();
            foreach (string guid in UnityEditor.AssetDatabase.FindAssets(assetSearchString))
            {
                T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));
                assets.Add(asset);
            }

            return assets;
#else
            return new List<T>();
#endif
        }

        public static List<T> FindAssets<T>(string name) where T : Object
        {
#if UNITY_EDITOR
            string assetSearchString = new AssetDatabaseSearchBuilder().
                       WithAssetName(name).
                       WithType<T>().
                       Build();

            List<T> assets = new List<T>();
            foreach (string guid in UnityEditor.AssetDatabase.FindAssets(assetSearchString))
            {
                T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));
                assets.Add(asset);
            }

            return assets;
#else
            return new List<T>();
#endif
        }

        public static T MustFindAsset<T>() where T : Object
        {
            return MustFindAsset<T>(string.Empty, string.Empty);
        }

        public static T MustFindAsset<T>(string name) where T : Object
        {
            return MustFindAsset<T>(name, string.Empty);
        }

        public static T MustFindAsset<T>(string name, string directory) where T : Object
        {
#if UNITY_EDITOR
            T asset = FindAsset<T>(name, directory);
            UnityEngine.Debug.Assert(asset != null, $"Could not find exactly one asset of type '{typeof(T).Name}' and name '{name}'.");
            return asset;
#else
            return default;
#endif
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
#if UNITY_EDITOR
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
#endif
            return default;
        }
    }
}
