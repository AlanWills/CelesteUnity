using Celeste.Events;
using Celeste.Events.AssetReferences;
using Celeste.Parameters.AssetReferences;
using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Sound.Settings
{
    [CreateAssetMenu(fileName = nameof(SFXSettingsUsingAddressables), menuName = "Celeste/Sound/SFX Settings Using Addressables")]
    public class SFXSettingsUsingAddressables : SFXSettings
    {
        #region Properties and Fields

        public override bool Enabled => enabled;

        [Header("Options")]
        [SerializeField] private BoolValueAssetReference sfxEnabled;

        [Header("Events")]
        [SerializeField] private AudioClipEventAssetReference playSFX;
        [SerializeField] private AudioClipEventAssetReference playSFXOneShot;

        [NonSerialized] private bool loaded = false;
        [NonSerialized] private bool enabled = false;

        #endregion

        public override bool ShouldLoadAssets()
        {
            return loaded;
        }

        public override IEnumerator LoadAssets()
        {
            yield return sfxEnabled.LoadAssetAsync();
            yield return playSFX.LoadAssetAsync();
            yield return playSFXOneShot.LoadAssetAsync();

            enabled = sfxEnabled.Asset.Value;

            AddOnSFXEnabledChangedCallback(OnSFXEnabledChanged);

            loaded = true;
        }

        public override void AddOnSFXEnabledChangedCallback(Action<bool> callback)
        {
            sfxEnabled.Asset.AddOnValueChangedCallback(callback);
        }

        public override void RemoveOnSFXEnabledChangedCallback(Action<bool> callback)
        {
            sfxEnabled.Asset.RemoveOnValueChangedCallback(callback);
        }

        public override void AddOnPlaySFXCallback(Action<AudioClip> callback)
        {
            playSFX.Asset.AddListener(callback);
        }

        public override void RemoveOnPlaySFXCallback(Action<AudioClip> callback)
        {
            playSFX.Asset.RemoveListener(callback);
        }

        public override void AddOnPlaySFXOneShotCallback(Action<AudioClip> callback)
        {
            playSFXOneShot.Asset.AddListener(callback);
        }

        public override void RemoveOnPlaySFXOneShotCallback(Action<AudioClip> callback)
        {
            playSFXOneShot.Asset.RemoveListener(callback);
        }

        #region Callbacks

        private void OnSFXEnabledChanged(bool newValue)
        {
            enabled = newValue;
        }

        #endregion
    }
}
