using Celeste.Assets;
using Celeste.Parameters;
using System;
using System.Collections;
using System.Collections.Generic;
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

        public abstract void AddOnMusicEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback);
        public abstract void RemoveOnMusicEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback);

        public abstract void AddOnPlayMusicCallback(Action<AudioClip> callback);
        public abstract void RemoveOnPlayMusicCallback(Action<AudioClip> callback);

        public abstract void AddOnPlayMusicOneShotCallback(Action<AudioClip> callback);
        public abstract void RemoveOnPlayMusicOneShotCallback(Action<AudioClip> callback);
    }
}
