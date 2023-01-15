using Celeste.Debug.Settings;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CelesteEditor.Debug.Settings
{
    public class DebugEditorSettingsProvider : SettingsProvider
    {
        #region Styles

        private class DebugSettingStyles
        {
            public static GUIContent isDebugBuildValue = new GUIContent("Is Debug Build Value");
        }

        #endregion

        #region Properties and Fields

        private SerializedObject debugSettings;
        private SerializedProperty isDebugBuildValueProperty;

        #endregion

        public DebugEditorSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(path, scope) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            debugSettings = DebugEditorSettings.GetSerializedSettings();
            isDebugBuildValueProperty = debugSettings.FindProperty(nameof(DebugEditorSettings.isDebugBuildValue));
        }

        public override void OnGUI(string searchContext)
        {
            debugSettings.Update();

            EditorGUILayout.PropertyField(isDebugBuildValueProperty);

            debugSettings.ApplyModifiedProperties();
        }

        #region Settings Provider

        public static bool IsSettingsAvailable()
        {
            return true;
        }

        // Register the SettingsProvider
        [SettingsProvider]
        public static SettingsProvider CreateLocalisationSettingsProvider()
        {
            if (IsSettingsAvailable())
            {
                var provider = new DebugEditorSettingsProvider("Project/Celeste/Debug Settings", SettingsScope.Project);

                // Automatically extract all keywords from the Styles.
                provider.keywords = GetSearchKeywordsFromGUIContentProperties<DebugSettingStyles>();
                return provider;
            }

            // Settings Asset doesn't exist yet; no need to display anything in the Settings window.
            return null;
        }

        #endregion
    }
}