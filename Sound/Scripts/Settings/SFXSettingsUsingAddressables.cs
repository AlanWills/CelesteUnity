using Celeste.Events;
using Celeste.Events.AssetReferences;
using Celeste.Parameters;
using Celeste.Parameters.AssetReferences;
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

        public override void AddOnSFXEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback)
        {
            sfxEnabled.Asset.AddValueChangedCallback(callback);
        }

        public override void RemoveOnSFXEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback)
        {
            sfxEnabled.Asset.RemoveValueChangedCallback(callback);
        }

        public override void AddOnPlaySFXCallback(UnityAction<AudioClip> callback)
        {
            playSFX.Asset.AddListener(callback);
        }

        public override void RemoveOnPlaySFXCallback(UnityAction<AudioClip> callback)
        {
            playSFX.Asset.RemoveListener(callback);
        }

        public override void AddOnPlaySFXOneShotCallback(UnityAction<AudioClip> callback)
        {
            playSFXOneShot.Asset.AddListener(callback);
        }

        public override void RemoveOnPlaySFXOneShotCallback(UnityAction<AudioClip> callback)
        {
            playSFXOneShot.Asset.RemoveListener(callback);
        }

        #region Callbacks

        private void OnSFXEnabledChanged(ValueChangedArgs<bool> args)
        {
            enabled = args.newValue;
        }

        #endregion
    }
}
