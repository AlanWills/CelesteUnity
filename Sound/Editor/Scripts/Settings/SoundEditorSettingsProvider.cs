using Celeste.Sound.Settings;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CelesteEditor.Localisation.Settings
{
    public class SoundEditorSettingsProvider : SettingsProvider
    {
        #region Styles

        private class SoundSettingStyles
        {
            public static GUIContent playMusicEvent = new GUIContent("Play Music Event");
            public static GUIContent playMusicOneShotEvent = new GUIContent("Play Music One Shot Event");
            public static GUIContent playSFXEvent = new GUIContent("Play SFX Event");
            public static GUIContent playSFXOneShotEvent = new GUIContent("Play SFX One Shot Event");
        }

        #endregion

        #region Properties and Fields

        private SerializedObject soundSettings;
        private SerializedProperty playMusicEventProperty;
        private SerializedProperty playMusicOneShotEventProperty;
        private SerializedProperty playSFXEventProperty;
        private SerializedProperty playSFXOneShotEventProperty;

        #endregion

        public SoundEditorSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(path, scope) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            soundSettings = SoundEditorSettings.GetSerializedSettings();
            playMusicEventProperty = soundSettings.FindProperty(nameof(SoundEditorSettings.playMusicEvent));
            playMusicOneShotEventProperty = soundSettings.FindProperty(nameof(SoundEditorSettings.playMusicOneShotEvent));
            playSFXEventProperty = soundSettings.FindProperty(nameof(SoundEditorSettings.playSFXEvent));
            playSFXOneShotEventProperty = soundSettings.FindProperty(nameof(SoundEditorSettings.playSFXOneShotEvent));
        }

        public override void OnGUI(string searchContext)
        {
            soundSettings.Update();

            EditorGUILayout.PropertyField(playMusicEventProperty);
            EditorGUILayout.PropertyField(playMusicOneShotEventProperty);
            EditorGUILayout.PropertyField(playSFXEventProperty);
            EditorGUILayout.PropertyField(playSFXOneShotEventProperty);

            soundSettings.ApplyModifiedProperties();
        }

        #region Settings Provider

        public static bool IsSettingsAvailable()
        {
            return true;
        }

        // Register the SettingsProvider
        [SettingsProvider]
        public static SettingsProvider CreateSoundSettingsProvider()
        {
            if (IsSettingsAvailable())
            {
                var provider = new SoundEditorSettingsProvider("Project/Celeste/Sound Settings", SettingsScope.Project);

                // Automatically extract all keywords from the Styles.
                provider.keywords = GetSearchKeywordsFromGUIContentProperties<SoundSettingStyles>();
                return provider;
            }

            // Settings Asset doesn't exist yet; no need to display anything in the Settings window.
            return null;
        }

        #endregion
    }
}