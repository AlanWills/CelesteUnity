using Celeste.Scene;
using Celeste.Scene.Events;
using Celeste.Tools.Settings;
using UnityEngine;
using Celeste;
using Celeste.Tools;

namespace CelesteEditor.Scene.Settings
{
    [CreateAssetMenu(fileName = nameof(SceneEditorSettings), order = CelesteMenuItemConstants.SCENE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.SCENE_MENU_ITEM + "Scene Editor Settings")]
    public class SceneEditorSettings : EditorSettings<SceneEditorSettings>
    {
        #region Properties and Fields

        public const string FOLDER_PATH = "Assets/Editor/Data/";
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

            defaultContextProvider = EditorOnly.MustFindAsset<ContextProvider>("DefaultContextProvider");
            defaultLoadContextEvent = EditorOnly.MustFindAsset<LoadContextEvent>("LoadContext");
        }
#endif

        #endregion
    }
}
