using Celeste.DataImporters.Settings;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CelesteEditor.DataImporters.Settings
{
    public class DataImporterEditorSettingsProvider : SettingsProvider
    {
        #region Styles

        private class PlatformSettingStyles
        {
            public static GUIContent dataImporterCatalogue = new GUIContent("Data Importer Catalogue");
        }

        #endregion

        #region Properties and Fields

        private SerializedObject dataImporterEditorSettings;
        private SerializedProperty dataImporterCatalogueProperty;

        #endregion

        public DataImporterEditorSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(path, scope) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            // This function is called when the user clicks on the MyCustom element in the Settings window.
            dataImporterEditorSettings = DataImporterEditorSettings.GetSerializedSettings();
            dataImporterCatalogueProperty = dataImporterEditorSettings.FindProperty(nameof(DataImporterEditorSettings.dataImporterCatalogue));
        }

        public override void OnGUI(string searchContext)
        {
            dataImporterEditorSettings.Update();

            EditorGUILayout.PropertyField(dataImporterCatalogueProperty);

            dataImporterEditorSettings.ApplyModifiedProperties();
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
                var provider = new DataImporterEditorSettingsProvider("Project/Celeste/Data Importer Settings", SettingsScope.Project);

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
