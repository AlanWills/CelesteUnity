using Celeste.Events;
using Celeste.Tools.Settings;
using UnityEngine;

namespace Celeste.Sound.Settings
{
    [CreateAssetMenu(fileName = nameof(SoundEditorSettings), menuName = "Celeste/Sound/Sound Editor Settings")]
    public class SoundEditorSettings : EditorSettings<SoundEditorSettings>
    {
        #region Properties and Fields

        public const string FOLDER_PATH = "Assets/Sound/Editor/Data/";
        public const string FILE_PATH = FOLDER_PATH + "SoundEditorSettings.asset";

        public AudioClipEvent playMusicEvent;
        public AudioClipEvent playMusicOneShotEvent;
        public AudioClipEvent playSFXEvent;
        public AudioClipEvent playSFXOneShotEvent;

        #endregion

#if UNITY_EDITOR
        public static SoundEditorSettings GetOrCreateSettings()
        {
            return GetOrCreateSettings(FOLDER_PATH, FILE_PATH);
        }

        public static UnityEditor.SerializedObject GetSerializedSettings()
        {
            return GetSerializedSettings(FOLDER_PATH, FILE_PATH);
        }
#endif
    }
}