﻿using System.IO;
using UnityEngine;

namespace Celeste.Tools.Settings
{
    public abstract class EditorSettings<T> : ScriptableObject where T : EditorSettings<T>
    {
#if UNITY_EDITOR
        protected static T GetOrCreateSettings(string folderPath, string filePath)
        {
            var settings = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(filePath);
            if (settings == null)
            {
                Debug.Log($"Creating instance of {typeof(T).Name} because it could not be found at {filePath}.");
                settings = CreateInstance<T>();

                Directory.CreateDirectory(folderPath);
                UnityEditor.AssetDatabase.Refresh();
                UnityEditor.AssetDatabase.CreateAsset(settings, filePath);
                UnityEditor.AssetDatabase.SaveAssets();
                
                settings.OnCreate();
                UnityEditor.EditorUtility.SetDirty(settings);
                UnityEditor.AssetDatabase.SaveAssetIfDirty(settings);
            }
            return settings;
        }

        protected static UnityEditor.SerializedObject GetSerializedSettings(string folderPath, string filePath)
        {
            return new UnityEditor.SerializedObject(GetOrCreateSettings(folderPath, filePath));
        }

        protected virtual void OnCreate() { }
#endif
    }
}
