﻿using Celeste.Assets.UnityAssetReferences;
using Celeste.Events.AssetReferences;
using Celeste.Parameters.AssetReferences;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Celeste.Sound.Settings
{
    [CreateAssetMenu(fileName = nameof(MusicSettingsUsingAddressables), menuName = "Celeste/Sound/Music Settings Using Addressables")]
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
        [SerializeField] private AudioClipEventAssetReference playMusic;
        [SerializeField] private AudioClipEventAssetReference playMusicOneShot;

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
            yield return playMusic.LoadAssetAsync();
            yield return playMusicOneShot.LoadAssetAsync();

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

        public override void AddOnMusicEnabledChangedCallback(Action<bool> callback)
        {
            musicEnabled.Asset.AddOnValueChangedCallback(callback);
        }

        public override void RemoveOnMusicEnabledChangedCallback(Action<bool> callback)
        {
            musicEnabled.Asset.RemoveOnValueChangedCallback(callback);
        }

        public override void AddOnPlayMusicCallback(Action<AudioClip> callback)
        {
            playMusic.Asset.AddListener(callback);
        }

        public override void RemoveOnPlayMusicCallback(Action<AudioClip> callback)
        {
            playMusic.Asset.RemoveListener(callback);
        }

        public override void AddOnPlayMusicOneShotCallback(Action<AudioClip> callback)
        {
            playMusicOneShot.Asset.AddListener(callback);
        }

        public override void RemoveOnPlayMusicOneShotCallback(Action<AudioClip> callback)
        {
            playMusicOneShot.Asset.RemoveListener(callback);
        }

        #region Callbacks

        private void OnMusicEnabledChanged(bool newValue)
        {
            enabled = newValue;
        }

        #endregion
    }
}