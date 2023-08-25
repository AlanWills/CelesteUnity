using Celeste.Assets;
using Celeste.Events;
using Celeste.Parameters;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Sound.Settings
{
    public abstract class SFXSettings : ScriptableObject, IHasAssets
    {
        #region Properties and Fields

        public abstract bool Enabled { get; }

        #endregion

        public abstract bool ShouldLoadAssets();
        public abstract IEnumerator LoadAssets();

        public void SetupListener(ISFXListener sfxListener)
        {
            AddOnSFXEnabledChangedCallback(sfxListener.OnSFXEnabledChanged);
            AddOnPlaySFXWithRawClipCallback(sfxListener.Play);
            AddOnPlaySFXWithSettingsCallback(sfxListener.Play);
            AddOnPlaySFXOneShotWithRawClipCallback(sfxListener.PlayOneShot);
            AddOnPlaySFXOneShotWithSettingsCallback(sfxListener.PlayOneShot);
        }

        public void ShutdownListener(ISFXListener sfxListener)
        {
            RemoveOnSFXEnabledChangedCallback(sfxListener.OnSFXEnabledChanged);
            RemoveOnPlaySFXWithRawClipCallback(sfxListener.Play);
            RemoveOnPlaySFXWithSettingsCallback(sfxListener.Play);
            RemoveOnPlaySFXOneShotWithRawClipCallback(sfxListener.PlayOneShot);
            RemoveOnPlaySFXOneShotWithSettingsCallback(sfxListener.PlayOneShot);
        }

        protected abstract void AddOnSFXEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback);
        protected abstract void RemoveOnSFXEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback);

        protected abstract void AddOnPlaySFXWithRawClipCallback(UnityAction<AudioClip> callback);
        protected abstract void RemoveOnPlaySFXWithRawClipCallback(UnityAction<AudioClip> callback);

        protected abstract void AddOnPlaySFXWithSettingsCallback(UnityAction<AudioClipSettings> callback);
        protected abstract void RemoveOnPlaySFXWithSettingsCallback(UnityAction<AudioClipSettings> callback);

        protected abstract void AddOnPlaySFXOneShotWithRawClipCallback(UnityAction<AudioClip> callback);
        protected abstract void RemoveOnPlaySFXOneShotWithRawClipCallback(UnityAction<AudioClip> callback);

        protected abstract void AddOnPlaySFXOneShotWithSettingsCallback(UnityAction<AudioClipSettings> callback);
        protected abstract void RemoveOnPlaySFXOneShotWithSettingsCallback(UnityAction<AudioClipSettings> callback);
    }
}
