using Celeste.Localisation.Settings;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CelesteEditor.Localisation.Settings
{
    public class LocalisationEditorSettingsProvider : SettingsProvider
    {
        #region Styles

        private class LocalisationSettingStyles
        {
            public static GUIContent currentLanguageValue = new GUIContent("Current Language Value");
            public static GUIContent localisationKeyCatalogue = new GUIContent("Localisation Key Catalogue");
        }

        #endregion

        #region Properties and Fields

        private SerializedObject localisationSettings;
        private SerializedProperty currentLanguageValueProperty;
        private SerializedProperty localisationKeyCatalogueProperty;

        #endregion

        public LocalisationEditorSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(path, scope) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            // This function is called when the user clicks on the MyCustom element in the Settings window.
            localisationSettings = LocalisationEditorSettings.GetSerializedSettings();
            currentLanguageValueProperty = localisationSettings.FindProperty(nameof(LocalisationEditorSettings.currentLanguageValue));
            localisationKeyCatalogueProperty = localisationSettings.FindProperty(nameof(LocalisationEditorSettings.localisationKeyCatalogue));
        }

        public override void OnGUI(string searchContext)
        {
            localisationSettings.Update();

            EditorGUILayout.PropertyField(currentLanguageValueProperty);
            EditorGUILayout.PropertyField(localisationKeyCatalogueProperty);

            localisationSettings.ApplyModifiedProperties();
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
                var provider = new LocalisationEditorSettingsProvider("Project/Celeste/Localisation Settings", SettingsScope.Project);

                // Automatically extract all keywords from the Styles.
                provider.keywords = GetSearchKeywordsFromGUIContentProperties<LocalisationSettingStyles>();
                return provider;
            }

            // Settings Asset doesn't exist yet; no need to display anything in the Settings window.
            return null;
        }

        #endregion
    }
}