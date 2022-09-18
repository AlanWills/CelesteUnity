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
        [SerializeField] private AudioClipEvent playMusic;
        [SerializeField] private AudioClipEvent playMusicOneShot;

        [Space]
        [SerializeField] private List<AudioClip> musicTracks = new List<AudioClip>();

        #endregion

        public override bool ShouldLoadAssets()
        {
            return false;
        }

        public override IEnumerator LoadAssets()
        {
            yield break;
        }

        public override void AddOnMusicEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback)
        {
            musicEnabled.AddValueChangedCallback(callback);
        }

        public override void RemoveOnMusicEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback)
        {
            musicEnabled.RemoveValueChangedCallback(callback);
        }

        public override void AddOnPlayMusicCallback(UnityAction<AudioClip> callback)
        {
            playMusic.AddListener(callback);
        }

        public override void RemoveOnPlayMusicCallback(UnityAction<AudioClip> callback)
        {
            playMusic.RemoveListener(callback);
        }

        public override void AddOnPlayMusicOneShotCallback(UnityAction<AudioClip> callback)
        {
            playMusicOneShot.AddListener(callback);
        }

        public override void RemoveOnPlayMusicOneShotCallback(UnityAction<AudioClip> callback)
        {
            playMusicOneShot.RemoveListener(callback);
        }
    }
}
