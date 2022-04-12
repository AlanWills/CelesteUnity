using Celeste.Assets;
using Celeste.Sound.Settings;
using Celeste.Tools;
using System.Collections;
using UnityEngine;

namespace Celeste.Sound
{
    [AddComponentMenu("Celeste/Sound/SFX Manager")]
    [RequireComponent(typeof(AudioSource))]
    public class SFXManager : MonoBehaviour, IHasAssets
    {
        #region Properties and Fields

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private SFXSettings sfxSettings;

        #endregion

        #region Unity Methods

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

            sfxSettings.AddOnPlaySFXCallback(Play);
            sfxSettings.AddOnPlaySFXOneShotCallback(PlayOneShot);
        }

        #endregion

        #region Callback

        private void Play(AudioClip audioClip)
        {
            if (sfxSettings.Enabled && !audioSource.isPlaying)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }

        private void PlayOneShot(AudioClip audioClip)
        {
            if (sfxSettings.Enabled)
            {
                audioSource.PlayOneShot(audioClip);
            }
        }

        #endregion
    }
}
