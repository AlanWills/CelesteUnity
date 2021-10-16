using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Robbi.Sound
{
    [AddComponentMenu("Celeste/Sound/SFX Manager")]
    [RequireComponent(typeof(AudioSource))]
    public class SFXManager : MonoBehaviour
    {
        #region Properties and Fields

        public AudioSource audioSource;
        public BoolValue sfxEnabled;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }

        #endregion

        #region Audio Methods

        public void Play(AudioClip audioClip)
        {
            if (sfxEnabled.Value && !audioSource.isPlaying)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }

        public void PlayOneShot(AudioClip audioClip)
        {
            if (sfxEnabled.Value)
            {
                audioSource.PlayOneShot(audioClip);
            }
        }

        public void OnSFXEnabledChanged(bool isEnabled)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).gameObject.SetActive(isEnabled);
            }
        }

        #endregion
    }
}
