using Celeste.Events;
using Celeste.Parameters.AssetReferences;
using Celeste.Sound.AssetReferences;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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
        [SerializeField] private AudioClipEventAssetReference playSFXWithRawClip;
        [SerializeField] private AudioClipSettingsEventAssetReference playSFXWithSettings;
        [SerializeField] private AudioClipEventAssetReference playSFXOneShotWithRawClip;
        [SerializeField] private AudioClipSettingsEventAssetReference playSFXOneShotWithSettings;

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
            yield return playSFXWithRawClip.LoadAssetAsync();
            yield return playSFXOneShotWithRawClip.LoadAssetAsync();

            enabled = sfxEnabled.Asset.Value;

            AddOnSFXEnabledChangedCallback(OnSFXEnabledChanged);

            loaded = true;
        }

        protected override void AddOnSFXEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback)
        {
            sfxEnabled.Asset.AddValueChangedCallback(callback);
        }

        protected override void RemoveOnSFXEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback)
        {
            sfxEnabled.Asset.RemoveValueChangedCallback(callback);
        }

        protected override void AddOnPlaySFXWithRawClipCallback(UnityAction<AudioClip> callback)
        {
            playSFXWithRawClip.Asset.AddListener(callback);
        }

        protected override void RemoveOnPlaySFXWithRawClipCallback(UnityAction<AudioClip> callback)
        {
            playSFXWithRawClip.Asset.RemoveListener(callback);
        }

        protected override void AddOnPlaySFXWithSettingsCallback(UnityAction<AudioClipSettings> callback)
        {
            playSFXWithSettings.Asset.AddListener(callback);
        }

        protected override void RemoveOnPlaySFXWithSettingsCallback(UnityAction<AudioClipSettings> callback)
        {
            playSFXWithSettings.Asset.RemoveListener(callback);
        }

        protected override void AddOnPlaySFXOneShotWithRawClipCallback(UnityAction<AudioClip> callback)
        {
            playSFXOneShotWithRawClip.Asset.AddListener(callback);
        }

        protected override void RemoveOnPlaySFXOneShotWithRawClipCallback(UnityAction<AudioClip> callback)
        {
            playSFXOneShotWithRawClip.Asset.RemoveListener(callback);
        }

        protected override void AddOnPlaySFXOneShotWithSettingsCallback(UnityAction<AudioClipSettings> callback)
        {
            playSFXOneShotWithSettings.Asset.AddListener(callback);
        }

        protected override void RemoveOnPlaySFXOneShotWithSettingsCallback(UnityAction<AudioClipSettings> callback)
        {
            playSFXOneShotWithSettings.Asset.RemoveListener(callback);
        }

        #region Callbacks

        private void OnSFXEnabledChanged(ValueChangedArgs<bool> args)
        {
            enabled = args.newValue;
        }

        #endregion
    }
}
