using Celeste.Scene;
using Celeste.Scene.Events;
using Celeste.Tools.Settings;
using UnityEngine;

namespace CelesteEditor.Scene.Settings
{
    [CreateAssetMenu(fileName = nameof(SceneEditorSettings), menuName = "Celeste/Scene/Scene Editor Settings")]
    public class SceneEditorSettings : EditorSettings<SceneEditorSettings>
    {
        #region Properties and Fields

        public const string FOLDER_PATH = "Assets/Scene/Editor/Data/";
        public const string FILE_PATH = FOLDER_PATH + "SceneEditorSettings.asset";

        public ContextProvider defaultContextProvider;
        public LoadContextEvent defaultLoadContextEvent;

        #endregion

        #region Editor Only
#if UNITY_EDITOR
        public static SceneEditorSettings GetOrCreateSettings()
        {
            return GetOrCreateSettings(FOLDER_PATH, FILE_PATH);
        }
#endif
        #endregion
    }
}
