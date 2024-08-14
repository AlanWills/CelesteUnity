#if USE_ADDRESSABLES
using Celeste.Assets.UnityAssetReferences;
using Celeste.Events;
using Celeste.Parameters.AssetReferences;
using Celeste.Sound.AssetReferences;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Sound.Settings
{
    [CreateAssetMenu(fileName = nameof(MusicSettingsUsingAddressables), order = CelesteMenuItemConstants.SOUND_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.SOUND_MENU_ITEM + "Music Settings Using Addressables")]
    public class MusicSettingsUsingAddressables : MusicSettings
    {
        #region Properties and Fields

        public override bool Enabled => enabled;
        public override bool Shuffle => shuffle;
        public override ReadOnlyCollection<AudioClip> Tracks => new ReadOnlyCollection<AudioClip>(tracks);

        [Header("Options")]
        [SerializeField] private BoolValueAssetReference musicEnabled;
        [SerializeField] private BoolValueAssetReference shuffleMusic;

        [Header("Events")]
        [SerializeField] private AudioClipEventAssetReference playMusicWithRawClip;
        [SerializeField] private AudioClipSettingsEventAssetReference playMusicWithSettings;
        [SerializeField] private AudioClipEventAssetReference playMusicOneShotWithRawClip;
        [SerializeField] private AudioClipSettingsEventAssetReference playMusicOneShotWithSettings;

        [Space]
        [SerializeField] private List<AudioClipAssetReference> musicTracks = new List<AudioClipAssetReference>();

        [NonSerialized] private bool loaded = false;
        [NonSerialized] private bool enabled = false;
        [NonSerialized] private bool shuffle = false;
        [NonSerialized] private List<AudioClip> tracks = new List<AudioClip>();

        #endregion

        public override bool ShouldLoadAssets()
        {
            return loaded;
        }

        public override IEnumerator LoadAssets()
        {
            yield return musicEnabled.LoadAssetAsync();
            yield return shuffleMusic.LoadAssetAsync();
            yield return playMusicWithRawClip.LoadAssetAsync();
            yield return playMusicWithSettings.LoadAssetAsync();
            yield return playMusicOneShotWithRawClip.LoadAssetAsync();
            yield return playMusicOneShotWithSettings.LoadAssetAsync();

            for (int i = 0, n = musicTracks.Count; i < n; ++i)
            {
                yield return musicTracks[i].LoadAssetAsync();
            }

            enabled = musicEnabled.Asset.Value;
            shuffle = shuffleMusic.Asset.Value;

            tracks.Clear();
            for (int i = 0, n = musicTracks.Count; i < n; ++i)
            {
                if (musicTracks[i].Asset != null)
                {
                    tracks.Add(musicTracks[i].Asset);
                }
            }

            AddOnMusicEnabledChangedCallback(OnMusicEnabledChanged);

            loaded = true;
        }

        protected override void AddOnMusicEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback)
        {
            musicEnabled.Asset.AddValueChangedCallback(callback);
        }

        protected override void RemoveOnMusicEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback)
        {
            musicEnabled.Asset.RemoveValueChangedCallback(callback);
        }

        protected override void AddOnPlayMusicWithRawClipCallback(UnityAction<AudioClip> callback)
        {
            playMusicWithRawClip.Asset.AddListener(callback);
        }

        protected override void RemoveOnPlayMusicWithRawClipCallback(UnityAction<AudioClip> callback)
        {
            playMusicWithRawClip.Asset.RemoveListener(callback);
        }

        protected override void AddOnPlayMusicWithSettingsCallback(UnityAction<AudioClipSettings> callback)
        {
            playMusicOneShotWithSettings.Asset.AddListener(callback);
        }

        protected override void RemoveOnPlayMusicWithSettingsCallback(UnityAction<AudioClipSettings> callback)
        {
            playMusicOneShotWithSettings.Asset.RemoveListener(callback);
        }

        protected override void AddOnPlayMusicOneShotWithRawClipCallback(UnityAction<AudioClip> callback)
        {
            playMusicOneShotWithRawClip.Asset.AddListener(callback);
        }

        protected override void RemoveOnPlayMusicOneShotWithRawClipCallback(UnityAction<AudioClip> callback)
        {
            playMusicOneShotWithRawClip.Asset.RemoveListener(callback);
        }

        protected override void AddOnPlayMusicOneShotWithSettingsCallback(UnityAction<AudioClipSettings> callback)
        {
            playMusicOneShotWithSettings.Asset.AddListener(callback);
        }

        protected override void RemoveOnPlayMusicOneShotWithSettingsCallback(UnityAction<AudioClipSettings> callback)
        {
            playMusicOneShotWithSettings.Asset.RemoveListener(callback);
        }

        #region Callbacks

        private void OnMusicEnabledChanged(ValueChangedArgs<bool> args)
        {
            enabled = args.newValue;
        }

        #endregion
    }
}
#endif