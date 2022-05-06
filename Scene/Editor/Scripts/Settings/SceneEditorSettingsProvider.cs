using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CelesteEditor.Scene.Settings
{
    public class SceneEditorSettingsProvider : SettingsProvider
    {
        #region Styles

        private class PlatformSettingStyles
        {
            public static GUIContent defaultContextProvider = new GUIContent("Default Context Provider");
            public static GUIContent defaultLoadContextEvent = new GUIContent("Default Load Context Event");

        }

        #endregion

        #region Properties and Fields

        private SerializedObject sceneEditorSettings;
        private SerializedProperty defaultContextProviderProperty;
        private SerializedProperty defaultLoadContextEventProperty;

        #endregion

        public SceneEditorSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(path, scope) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            sceneEditorSettings = SceneEditorSettings.GetSerializedSettings();
            defaultContextProviderProperty = sceneEditorSettings.FindProperty("defaultContextProvider");
            defaultLoadContextEventProperty = sceneEditorSettings.FindProperty("defaultLoadContextEvent");
        }

        public override void OnGUI(string searchContext)
        {
            sceneEditorSettings.Update();

            EditorGUILayout.PropertyField(defaultContextProviderProperty);
            EditorGUILayout.PropertyField(defaultLoadContextEventProperty);

            sceneEditorSettings.ApplyModifiedProperties();
        }

        #region Settings Provider

        public static bool IsSettingsAvailable()
        {
            return true;
        }

        [SettingsProvider]
        public static SettingsProvider CreateSceneSettingsProvider()
        {
            if (IsSettingsAvailable())
            {
                var provider = new SceneEditorSettingsProvider("Project/Celeste/Scene Settings", SettingsScope.Project);
                provider.keywords = GetSearchKeywordsFromGUIContentProperties<PlatformSettingStyles>();

                return provider;
            }

            return null;
        }

        #endregion
    }
}