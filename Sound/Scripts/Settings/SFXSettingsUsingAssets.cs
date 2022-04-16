using Celeste.Events;
using Celeste.Events.AssetReferences;
using Celeste.Parameters;
using Celeste.Parameters.AssetReferences;
using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Sound.Settings
{
    [CreateAssetMenu(fileName = nameof(SFXSettingsUsingAssets), menuName = "Celeste/Sound/SFX Settings Using Assets")]
    public class SFXSettingsUsingAssets : SFXSettings
    {
        #region Properties and Fields

        public override bool Enabled => sfxEnabled.Value;

        [Header("Options")]
        [SerializeField] private BoolValue sfxEnabled;
        
        [Header("Events")]
        [SerializeField] private AudioClipEvent playSFX;
        [SerializeField] private AudioClipEvent playSFXOneShot;

        #endregion

        public override bool ShouldLoadAssets()
        {
            return false;
        }

        public override IEnumerator LoadAssets()
        {
            yield break;
        }

        public override void AddOnSFXEnabledChangedCallback(Action<bool> callback)
        {
            sfxEnabled.AddOnValueChangedCallback(callback);
        }

        public override void RemoveOnSFXEnabledChangedCallback(Action<bool> callback)
        {
            sfxEnabled.RemoveOnValueChangedCallback(callback);
        }

        public override void AddOnPlaySFXCallback(Action<AudioClip> callback)
        {
            playSFX.AddListener(callback);
        }

        public override void RemoveOnPlaySFXCallback(Action<AudioClip> callback)
        {
            playSFX.RemoveListener(callback);
        }

        public override void AddOnPlaySFXOneShotCallback(Action<AudioClip> callback)
        {
            playSFXOneShot.AddListener(callback);
        }

        public override void RemoveOnPlaySFXOneShotCallback(Action<AudioClip> callback)
        {
            playSFXOneShot.RemoveListener(callback);
        }
    }
}
