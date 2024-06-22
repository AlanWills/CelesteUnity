using System.IO;
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
                Directory.CreateDirectory(folderPath);

                Debug.Log($"Creating instance of {typeof(T).Name} because it could not be found at {filePath}.");
                settings = CreateInstance<T>();
                UnityEditor.AssetDatabase.CreateAsset(settings, filePath);
                
                settings.OnCreate();
                Debug.Assert(settings != null, $"Editor Settings {typeof(T).Name} was null in {nameof(GetOrCreateSettings)}.  This should never happen...");
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
