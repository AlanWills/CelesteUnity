using Celeste.Events;
using Celeste.Parameters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Sound.Settings
{
    [CreateAssetMenu(fileName = nameof(MusicSettingsUsingAssets), menuName = "Celeste/Sound/Music Settings Using Assets")]
    public class MusicSettingsUsingAssets : MusicSettings
    {
        #region Properties and Fields

        public override bool Enabled => musicEnabled.Value;
        public override bool Shuffle => shuffleMusic.Value;
        public override ReadOnlyCollection<AudioClip> Tracks => new ReadOnlyCollection<AudioClip>(musicTracks);

        [Header("Options")]
        [SerializeField] private BoolValue musicEnabled;
        [SerializeField] private BoolValue shuffleMusic;

        [Header("Events")]
        [SerializeField] private AudioClipEvent playMusicWithRawClip;
        [SerializeField] private AudioClipSettingsEvent playMusicWithSettings;
        [SerializeField] private AudioClipEvent playMusicOneShotWithRawClip;
        [SerializeField] private AudioClipSettingsEvent playMusicOneShotWithSettings;

        [Space]
        [SerializeField] private List<AudioClip> musicTracks = new List<AudioClip>();

        #endregion

        #region Unity Events

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (playMusicWithRawClip == null)
            {
                playMusicWithRawClip = SoundEditorSettings.GetOrCreateSettings().playMusicWithRawClipEvent;
                UnityEditor.EditorUtility.SetDirty(this);
            }

            if (playMusicWithSettings == null)
            {
                playMusicWithSettings = SoundEditorSettings.GetOrCreateSettings().playMusicWithSettingsEvent;
                UnityEditor.EditorUtility.SetDirty(this);
            }

            if (playMusicOneShotWithRawClip == null)
            {
                playMusicOneShotWithRawClip = SoundEditorSettings.GetOrCreateSettings().playMusicOneShotWithRawClipEvent;
                UnityEditor.EditorUtility.SetDirty(this);
            }

            if (playMusicOneShotWithSettings == null)
            {
                playMusicOneShotWithSettings = SoundEditorSettings.GetOrCreateSettings().playMusicOneShotWithSettingsEvent;
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
        }

        #endregion

        public override bool ShouldLoadAssets()
        {
            return false;
        }

        public override IEnumerator LoadAssets()
        {
            yield break;
        }

        protected override void AddOnMusicEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback)
        {
            musicEnabled.AddValueChangedCallback(callback);
        }

        protected override void RemoveOnMusicEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback)
        {
            musicEnabled.RemoveValueChangedCallback(callback);
        }

        protected override void AddOnPlayMusicWithRawClipCallback(UnityAction<AudioClip> callback)
        {
            playMusicWithRawClip.AddListener(callback);
        }

        protected override void RemoveOnPlayMusicWithRawClipCallback(UnityAction<AudioClip> callback)
        {
            playMusicWithRawClip.RemoveListener(callback);
        }

        protected override void AddOnPlayMusicWithSettingsCallback(UnityAction<AudioClipSettings> callback)
        {
            playMusicWithSettings.AddListener(callback);
        }

        protected override void RemoveOnPlayMusicWithSettingsCallback(UnityAction<AudioClipSettings> callback)
        {
            playMusicWithSettings.RemoveListener(callback);
        }

        protected override void AddOnPlayMusicOneShotWithRawClipCallback(UnityAction<AudioClip> callback)
        {
            playMusicOneShotWithRawClip.AddListener(callback);
        }

        protected override void RemoveOnPlayMusicOneShotWithRawClipCallback(UnityAction<AudioClip> callback)
        {
            playMusicOneShotWithRawClip.RemoveListener(callback);
        }

        protected override void AddOnPlayMusicOneShotWithSettingsCallback(UnityAction<AudioClipSettings> callback)
        {
            playMusicOneShotWithSettings.AddListener(callback);
        }

        protected override void RemoveOnPlayMusicOneShotWithSettingsCallback(UnityAction<AudioClipSettings> callback)
        {
            playMusicOneShotWithSettings.RemoveListener(callback);
        }
    }
}
