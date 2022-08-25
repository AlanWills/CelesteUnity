using Celeste.Persistence.Settings;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CelesteEditor.Persistence.Settings
{
    public class PersistenceEditorSettingsProvider : SettingsProvider
    {
        #region Styles

        private class PersistenceSettingStyles
        {
            public static GUIContent snapshotRecord = new GUIContent("Snapshot Record");
        }

        #endregion

        #region Properties and Fields

        private SerializedObject persistenceSettings;
        private SerializedProperty snapshotRecordProperty;

        #endregion

        public PersistenceEditorSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(path, scope) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            // This function is called when the user clicks on the MyCustom element in the Settings window.
            persistenceSettings = PersistenceEditorSettings.GetSerializedSettings();
            snapshotRecordProperty = persistenceSettings.FindProperty(nameof(PersistenceEditorSettings.snapshotRecord));
        }

        public override void OnGUI(string searchContext)
        {
            persistenceSettings.Update();

            EditorGUILayout.PropertyField(snapshotRecordProperty);

            persistenceSettings.ApplyModifiedProperties();
        }

        #region Settings Provider

        public static bool IsSettingsAvailable()
        {
            return true;
        }

        // Register the SettingsProvider
        [SettingsProvider]
        public static SettingsProvider CreatePersistenceSettingsProvider()
        {
            if (IsSettingsAvailable())
            {
                var provider = new PersistenceEditorSettingsProvider("Project/Celeste/Persistence Settings", SettingsScope.Project);

                // Automatically extract all keywords from the Styles.
                provider.keywords = GetSearchKeywordsFromGUIContentProperties<PersistenceSettingStyles>();
                return provider;
            }

            // Settings Asset doesn't exist yet; no need to display anything in the Settings window.
            return null;
        }

        #endregion
    }
}