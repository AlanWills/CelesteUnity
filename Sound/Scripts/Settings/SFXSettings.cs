using Celeste.Assets;
using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Sound.Settings
{
    public abstract class SFXSettings : ScriptableObject, IHasAssets
    {
        #region Properties and Fields

        public abstract bool Enabled { get; }

        #endregion

        public abstract bool ShouldLoadAssets();
        public abstract IEnumerator LoadAssets();

        public abstract void AddOnSFXEnabledChangedCallback(Action<bool> callback);
        public abstract void RemoveOnSFXEnabledChangedCallback(Action<bool> callback);

        public abstract void AddOnPlaySFXCallback(Action<AudioClip> callback);
        public abstract void RemoveOnPlaySFXCallback(Action<AudioClip> callback);

        public abstract void AddOnPlaySFXOneShotCallback(Action<AudioClip> callback);
        public abstract void RemoveOnPlaySFXOneShotCallback(Action<AudioClip> callback);
    }
}
