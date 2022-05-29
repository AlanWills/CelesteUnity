using Celeste.LiveOps.Settings;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CelesteEditor.LiveOps.Settings
{
    public class LiveOpsEditorSettingsProvider : SettingsProvider
    {
        #region Styles

        private class PlatformSettingStyles
        {
            public static GUIContent defaultComponentCatalogue = new GUIContent("Default Component Catalogue");
        }

        #endregion

        #region Properties and Fields

        private SerializedObject liveOpsSettings;
        private SerializedProperty defaultComponentCatalogueProperty;

        #endregion

        public LiveOpsEditorSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(path, scope) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            // This function is called when the user clicks on the MyCustom element in the Settings window.
            liveOpsSettings = LiveOpsEditorSettings.GetSerializedSettings();
            defaultComponentCatalogueProperty = liveOpsSettings.FindProperty(nameof(LiveOpsEditorSettings.defaultComponentCatalogue));
        }

        public override void OnGUI(string searchContext)
        {
            liveOpsSettings.Update();

            EditorGUILayout.PropertyField(defaultComponentCatalogueProperty);

            liveOpsSettings.ApplyModifiedProperties();
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
                var provider = new LiveOpsEditorSettingsProvider("Project/Celeste/Live Ops Settings", SettingsScope.Project);

                // Automatically extract all keywords from the Styles.
                provider.keywords = GetSearchKeywordsFromGUIContentProperties<PlatformSettingStyles>();
                return provider;
            }

            // Settings Asset doesn't exist yet; no need to display anything in the Settings window.
            return null;
        }

        #endregion
    }
}