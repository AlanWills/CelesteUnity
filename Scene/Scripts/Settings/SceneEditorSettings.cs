using Celeste.Scene;
using Celeste.Scene.Events;
using Celeste.Tools.Settings;
using UnityEngine;
using Celeste;

#if UNITY_EDITOR
using CelesteEditor.Tools;
#endif

namespace CelesteEditor.Scene.Settings
{
    [CreateAssetMenu(fileName = nameof(SceneEditorSettings), order = CelesteMenuItemConstants.SCENE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.SCENE_MENU_ITEM + "Scene Editor Settings")]
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

        protected override void OnCreate()
        {
            base.OnCreate();

            defaultContextProvider = EditorOnly.FindAsset<ContextProvider>("DefaultContextProvider");
            defaultLoadContextEvent = EditorOnly.FindAsset<LoadContextEvent>("LoadContext");
        }
#endif

        #endregion
    }
}
