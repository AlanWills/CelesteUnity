using Celeste.Assets;
using Celeste.Events;
using Celeste.Sound.Settings;
using Celeste.Tools;
using System.Collections;
using UnityEngine;

namespace Celeste.Sound
{
    [AddComponentMenu("Celeste/Sound/SFX Manager")]
    [RequireComponent(typeof(AudioSource))]
    public class SFXManager : MonoBehaviour, IHasAssets, ISFXListener
    {
        #region Properties and Fields

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private SFXSettings sfxSettings;

        #endregion

        #region Unity Methods

        private void OnDisable()
        {
            sfxSettings.ShutdownListener(this);
        }

        private void OnValidate()
        {
            this.TryGet(ref audioSource);
        }

        #endregion

        #region IHasAssets

        public bool ShouldLoadAssets()
        {
            return sfxSettings.ShouldLoadAssets();
        }

        public IEnumerator LoadAssets()
        {
            yield return sfxSettings.LoadAssets();

            sfxSettings.SetupListener(this);
        }

        #endregion

        #region Callback

        public void OnSFXEnabledChanged(ValueChangedArgs<bool> args)
        {
        }

        public void Play(AudioClip audioClip)
        {
            if (sfxSettings.Enabled && !audioSource.isPlaying)
            {
                audioSource.volume = 1.0f;
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }

        public void Play(AudioClipSettings audioClipSettings)
        {
            if (sfxSettings.Enabled && !audioSource.isPlaying)
            {
                audioSource.volume = audioClipSettings.Volume;
                audioSource.clip = audioClipSettings.Clip;
                audioSource.Play();
            }
        }

        public void PlayOneShot(AudioClip audioClip)
        {
            if (sfxSettings.Enabled)
            {
                audioSource.volume = 1.0f;
                audioSource.PlayOneShot(audioClip);
            }
        }

        public void PlayOneShot(AudioClipSettings audioClipSettings)
        {
            if (sfxSettings.Enabled)
            {
                audioSource.volume = 1.0f;
                audioSource.PlayOneShot(audioClipSettings.Clip, audioClipSettings.Volume);
            }
        }

        #endregion
    }
}
