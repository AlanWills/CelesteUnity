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

        #region Unity Events

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (playSFX == null)
            {
                playSFX = SoundEditorSettings.GetOrCreateSettings().playSFXEvent;
                UnityEditor.EditorUtility.SetDirty(this);
            }

            if (playSFXOneShot == null)
            {
                playSFXOneShot = SoundEditorSettings.GetOrCreateSettings().playSFXOneShotEvent;
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
        }

        #endregion

        public override bool ShouldLoadAssets()
        {
            return false;
        }

        public override IEnumerator LoadAssets()
        {
            yield break;
        }

        public override void AddOnSFXEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback)
        {
            sfxEnabled.AddValueChangedCallback(callback);
        }

        public override void RemoveOnSFXEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback)
        {
            sfxEnabled.RemoveValueChangedCallback(callback);
        }

        public override void AddOnPlaySFXCallback(UnityAction<AudioClip> callback)
        {
            playSFX.AddListener(callback);
        }

        public override void RemoveOnPlaySFXCallback(UnityAction<AudioClip> callback)
        {
            playSFX.RemoveListener(callback);
        }

        public override void AddOnPlaySFXOneShotCallback(UnityAction<AudioClip> callback)
        {
            playSFXOneShot.AddListener(callback);
        }

        public override void RemoveOnPlaySFXOneShotCallback(UnityAction<AudioClip> callback)
        {
            playSFXOneShot.RemoveListener(callback);
        }
    }
}
