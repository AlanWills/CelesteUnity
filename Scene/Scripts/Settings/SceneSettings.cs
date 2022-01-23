#if UNITY_EDITOR
using Celeste.Scene.Events;
using UnityEditor;
using UnityEngine;

namespace Celeste.Scene.Settings
{
    [FilePath(SCENE_SETTINGS_FILE_PATH, FilePathAttribute.Location.ProjectFolder)]
    [CreateAssetMenu(fileName = nameof(SceneSettings), menuName = "Celeste/Scene/Scene Settings")]
    public class SceneSettings : ScriptableSingleton<SceneSettings>
    {
        public ContextProvider defaultContextProvider;
        public LoadContextEvent defaultLoadContextEvent;

        public const string SCENE_SETTINGS_FILE_PATH = "Assets/Celeste/Scene/Editor/Data/SceneSettings.asset";
    }
}
#endif