using Celeste.Assets;
using Celeste.Events;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Sound.Settings
{
    public abstract class MusicSettings : ScriptableObject, IHasAssets
    {
        #region Properties and Fields

        public abstract bool Enabled { get; }
        public abstract bool Shuffle { get; }
        public abstract ReadOnlyCollection<AudioClip> Tracks { get; }

        #endregion

        public abstract bool ShouldLoadAssets();
        public abstract IEnumerator LoadAssets();

        public void SetupListener(IMusicListener musicListener)
        {
            AddOnMusicEnabledChangedCallback(musicListener.OnMusicEnabledChanged);
            AddOnPlayMusicWithRawClipCallback(musicListener.Play);
            AddOnPlayMusicWithSettingsCallback(musicListener.Play);
            AddOnPlayMusicOneShotWithRawClipCallback(musicListener.PlayOneShot);
            AddOnPlayMusicOneShotWithSettingsCallback(musicListener.PlayOneShot);
        }

        public void ShutdownListener(IMusicListener musicListener)
        {
            RemoveOnMusicEnabledChangedCallback(musicListener.OnMusicEnabledChanged);
            RemoveOnPlayMusicWithRawClipCallback(musicListener.Play);
            RemoveOnPlayMusicWithSettingsCallback(musicListener.Play);
            RemoveOnPlayMusicOneShotWithRawClipCallback(musicListener.PlayOneShot);
            RemoveOnPlayMusicOneShotWithSettingsCallback(musicListener.PlayOneShot);
        }

        protected abstract void AddOnMusicEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback);
        protected abstract void RemoveOnMusicEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback);

        protected abstract void AddOnPlayMusicWithRawClipCallback(UnityAction<AudioClip> callback);
        protected abstract void RemoveOnPlayMusicWithRawClipCallback(UnityAction<AudioClip> callback);

        protected abstract void AddOnPlayMusicWithSettingsCallback(UnityAction<AudioClipSettings> callback);
        protected abstract void RemoveOnPlayMusicWithSettingsCallback(UnityAction<AudioClipSettings> callback);

        protected abstract void AddOnPlayMusicOneShotWithRawClipCallback(UnityAction<AudioClip> callback);
        protected abstract void RemoveOnPlayMusicOneShotWithRawClipCallback(UnityAction<AudioClip> callback);

        protected abstract void AddOnPlayMusicOneShotWithSettingsCallback(UnityAction<AudioClipSettings> callback);
        protected abstract void RemoveOnPlayMusicOneShotWithSettingsCallback(UnityAction<AudioClipSettings> callback);
    }
}
