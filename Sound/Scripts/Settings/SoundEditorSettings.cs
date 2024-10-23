using Celeste.Tools.Settings;
using UnityEngine;
using Celeste.Tools;
using Celeste.Parameters;

namespace Celeste.Sound.Settings
{
    [CreateAssetMenu(fileName = nameof(SoundEditorSettings), order = CelesteMenuItemConstants.SOUND_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.SOUND_MENU_ITEM + "Sound Editor Settings")]
    public class SoundEditorSettings : EditorSettings<SoundEditorSettings>
    {
        #region Properties and Fields

        public const string FOLDER_PATH = "Assets/Sound/Editor/Data/";
        public const string FILE_PATH = FOLDER_PATH + "SoundEditorSettings.asset";

        public BoolValue sfxEnabled;
        public BoolValue musicEnabled;
        public BoolValue shuffleMusic;

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

            sfxEnabled = EditorOnly.MustFindAsset<BoolValue>("SFXEnabled");
            musicEnabled = EditorOnly.MustFindAsset<BoolValue>("MusicEnabled");
            shuffleMusic = EditorOnly.MustFindAsset<BoolValue>("ShuffleMusic");

            playMusicWithRawClipEvent = EditorOnly.MustFindAsset<AudioClipEvent>("PlayMusicWithRawClip");
            playMusicWithSettingsEvent = EditorOnly.MustFindAsset<AudioClipSettingsEvent>("PlayMusicWithSettings");
            playMusicOneShotWithRawClipEvent = EditorOnly.MustFindAsset<AudioClipEvent>("PlayMusicOneShotWithRawClip");
            playMusicOneShotWithSettingsEvent = EditorOnly.MustFindAsset<AudioClipSettingsEvent>("PlayMusicOneShotWithSettings");

            playSFXWithRawClipEvent = EditorOnly.MustFindAsset<AudioClipEvent>("PlaySFXWithRawClip");
            playSFXWithSettingsEvent = EditorOnly.MustFindAsset<AudioClipSettingsEvent>("PlaySFXWithSettings");
            playSFXOneShotWithRawClipEvent = EditorOnly.MustFindAsset<AudioClipEvent>("PlaySFXOneShotWithRawClip");
            playSFXOneShotWithSettingsEvent = EditorOnly.MustFindAsset<AudioClipSettingsEvent>("PlaySFXOneShotWithSettings");
        }
#endif
    }
}