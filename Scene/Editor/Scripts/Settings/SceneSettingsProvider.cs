using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CelesteEditor.Scene.Settings
{
    public class SceneSettingsProvider : SettingsProvider
    {
        #region Styles

        private class PlatformSettingStyles
        {
            public static GUIContent defaultContextProvider = new GUIContent("Default Context Provider");
            public static GUIContent defaultLoadContextEvent = new GUIContent("Default Load Context Event");

        }

        #endregion

        #region Properties and Fields

        private SerializedObject sceneSettings;
        private SerializedProperty defaultContextProviderProperty;
        private SerializedProperty defaultLoadContextEventProperty;

        #endregion

        public SceneSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(path, scope) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            sceneSettings = new SerializedObject(SceneSettings.instance);
            defaultContextProviderProperty = sceneSettings.FindProperty("defaultContextProvider");
            defaultLoadContextEventProperty = sceneSettings.FindProperty("defaultLoadContextEvent");
        }

        public override void OnGUI(string searchContext)
        {
            sceneSettings.Update();

            EditorGUILayout.PropertyField(defaultContextProviderProperty);
            EditorGUILayout.PropertyField(defaultLoadContextEventProperty);

            sceneSettings.ApplyModifiedProperties();
        }

        #region Settings Provider

        public static bool IsSettingsAvailable()
        {
            return File.Exists(SceneSettings.SCENE_SETTINGS_FILE_PATH);
        }

        [SettingsProvider]
        public static SettingsProvider CreateSceneSettingsProvider()
        {
            if (IsSettingsAvailable())
            {
                var provider = new SceneSettingsProvider("Project/Celeste/Scene Settings", SettingsScope.Project);
                provider.keywords = GetSearchKeywordsFromGUIContentProperties<PlatformSettingStyles>();

                return provider;
            }

            return null;
        }

        #endregion
    }
}