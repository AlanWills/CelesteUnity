using Celeste.Assets;
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

        public abstract void AddOnSFXEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback);
        public abstract void RemoveOnSFXEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback);

        public abstract void AddOnPlaySFXCallback(Action<AudioClip> callback);
        public abstract void RemoveOnPlaySFXCallback(Action<AudioClip> callback);

        public abstract void AddOnPlaySFXOneShotCallback(Action<AudioClip> callback);
        public abstract void RemoveOnPlaySFXOneShotCallback(Action<AudioClip> callback);
    }
}
