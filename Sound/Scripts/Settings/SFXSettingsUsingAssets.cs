using Celeste.Events;
using Celeste.Parameters;
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
        [SerializeField] private AudioClipEvent playSFXWithRawClip;
        [SerializeField] private AudioClipSettingsEvent playSFXWithSettings;
        [SerializeField] private AudioClipEvent playSFXOneShotWithRawClip;
        [SerializeField] private AudioClipSettingsEvent playSFXOneShotWithSettings;

        #endregion

        #region Unity Events

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (playSFXWithRawClip == null)
            {
                playSFXWithRawClip = SoundEditorSettings.GetOrCreateSettings().playSFXWithRawClipEvent;
                UnityEditor.EditorUtility.SetDirty(this);
            }

            if (playSFXWithSettings == null)
            {
                playSFXWithSettings = SoundEditorSettings.GetOrCreateSettings().playSFXWithSettingsEvent;
                UnityEditor.EditorUtility.SetDirty(this);
            }

            if (playSFXOneShotWithRawClip == null)
            {
                playSFXOneShotWithRawClip = SoundEditorSettings.GetOrCreateSettings().playSFXOneShotWithRawClipEvent;
                UnityEditor.EditorUtility.SetDirty(this);
            }

            if (playSFXOneShotWithSettings == null)
            {
                playSFXOneShotWithSettings = SoundEditorSettings.GetOrCreateSettings().playSFXOneShotWithSettingsEvent;
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

        protected override void AddOnSFXEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback)
        {
            sfxEnabled.AddValueChangedCallback(callback);
        }

        protected override void RemoveOnSFXEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback)
        {
            sfxEnabled.RemoveValueChangedCallback(callback);
        }

        protected override void AddOnPlaySFXWithRawClipCallback(UnityAction<AudioClip> callback)
        {
            playSFXWithRawClip.AddListener(callback);
        }

        protected override void RemoveOnPlaySFXWithRawClipCallback(UnityAction<AudioClip> callback)
        {
            playSFXWithRawClip.RemoveListener(callback);
        }

        protected override void AddOnPlaySFXWithSettingsCallback(UnityAction<AudioClipSettings> callback)
        {
            playSFXWithSettings.AddListener(callback);
        }

        protected override void RemoveOnPlaySFXWithSettingsCallback(UnityAction<AudioClipSettings> callback)
        {
            playSFXWithSettings.RemoveListener(callback);
        }

        protected override void AddOnPlaySFXOneShotWithRawClipCallback(UnityAction<AudioClip> callback)
        {
            playSFXOneShotWithRawClip.AddListener(callback);
        }

        protected override void RemoveOnPlaySFXOneShotWithRawClipCallback(UnityAction<AudioClip> callback)
        {
            playSFXOneShotWithRawClip.RemoveListener(callback);
        }

        protected override void AddOnPlaySFXOneShotWithSettingsCallback(UnityAction<AudioClipSettings> callback)
        {
            playSFXOneShotWithSettings.AddListener(callback);
        }

        protected override void RemoveOnPlaySFXOneShotWithSettingsCallback(UnityAction<AudioClipSettings> callback)
        {
            playSFXOneShotWithSettings.RemoveListener(callback);
        }
    }
}
