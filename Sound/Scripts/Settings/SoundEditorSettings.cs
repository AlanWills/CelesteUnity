using Celeste.Tools.Settings;
using UnityEngine;
#if UNITY_EDITOR
using CelesteEditor.Tools;
#endif

namespace Celeste.Sound.Settings
{
    [CreateAssetMenu(fileName = nameof(SoundEditorSettings), order = CelesteMenuItemConstants.SOUND_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.SOUND_MENU_ITEM + "Sound Editor Settings")]
    public class SoundEditorSettings : EditorSettings<SoundEditorSettings>
    {
        #region Properties and Fields

        public const string FOLDER_PATH = "Assets/Sound/Editor/Data/";
        public const string FILE_PATH = FOLDER_PATH + "SoundEditorSettings.asset";

        public AudioClipEvent playMusicWithRawClipEvent;
        public AudioClipSettingsEvent playMusicWithSettingsEvent;
        public AudioClipEvent playMusicOneShotWithRawClipEvent;
        public AudioClipSettingsEvent playMusicOneShotWithSettingsEvent;

        public AudioClipEvent playSFXWithRawClipEvent;
        public AudioClipSettingsEvent playSFXWithSettingsEvent;
        public AudioClipEvent playSFXOneShotWithRawClipEvent;
        public AudioClipSettingsEvent playSFXOneShotWithSettingsEvent;

        #endregion

#if UNITY_EDITOR
        public static SoundEditorSettings GetOrCreateSettings()
        {
            return GetOrCreateSettings(FOLDER_PATH, FILE_PATH);
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            playMusicWithRawClipEvent = EditorOnly.FindAsset<AudioClipEvent>("PlayMusicWithRawClip");
            playMusicWithSettingsEvent = EditorOnly.FindAsset<AudioClipSettingsEvent>("PlayMusicWithSettings");
            playMusicOneShotWithRawClipEvent = EditorOnly.FindAsset<AudioClipEvent>("PlayMusicOneShotWithRawClip");
            playMusicOneShotWithSettingsEvent = EditorOnly.FindAsset<AudioClipSettingsEvent>("PlayMusicOneShotWithSettings");

            playSFXWithRawClipEvent = EditorOnly.FindAsset<AudioClipEvent>("PlaySFXWithRawClip");
            playSFXWithSettingsEvent = EditorOnly.FindAsset<AudioClipSettingsEvent>("PlaySFXWithSettings");
            playSFXOneShotWithRawClipEvent = EditorOnly.FindAsset<AudioClipEvent>("PlaySFXOneShotWithRawClip");
            playSFXOneShotWithSettingsEvent = EditorOnly.FindAsset<AudioClipSettingsEvent>("PlaySFXOneShotWithSettings");
        }
#endif
    }
}