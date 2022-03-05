using Celeste.Localisation.Settings;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CelesteEditor.Localisation.Settings
{
    public class LocalisationSettingsProvider : SettingsProvider
    {
        #region Styles

        private class PlatformSettingStyles
        {
            public static GUIContent currentLanguageValue = new GUIContent("Current Language Value");
            public static GUIContent postImportSteps = new GUIContent("Post Import Steps");
        }

        #endregion

        #region Properties and Fields

        private SerializedObject localisationSettings;
        private SerializedProperty currentLanguageValueProperty;
        private SerializedProperty postImportStepsProperty;

        #endregion

        public LocalisationSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(path, scope) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            // This function is called when the user clicks on the MyCustom element in the Settings window.
            localisationSettings = LocalisationSettings.GetSerializedSettings();
            currentLanguageValueProperty = localisationSettings.FindProperty(nameof(LocalisationSettings.currentLanguageValue));
            postImportStepsProperty = localisationSettings.FindProperty(nameof(LocalisationSettings.postImportSteps));
        }

        public override void OnGUI(string searchContext)
        {
            localisationSettings.Update();

            EditorGUILayout.PropertyField(currentLanguageValueProperty);
            EditorGUILayout.PropertyField(postImportStepsProperty);

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
                var provider = new LocalisationSettingsProvider("Project/Celeste/Localisation Settings", SettingsScope.Project);

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