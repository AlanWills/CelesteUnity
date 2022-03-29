using Celeste.Parameters;
using System.Collections.Generic;
using UnityEngine;

namespace Robbi.Sound
{
    [AddComponentMenu("Celeste/Sound/Music Manager")]
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private bool shuffle = true;
        [SerializeField] private List<AudioClip> music = new List<AudioClip>();
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private BoolValue musicEnabled;

        private int currentTrackIndex = -1;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (audioSource.playOnAwake)
            {
                NextTrack();
            }
        }

        private void OnValidate()
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }

        #endregion

        #region Utility

        public void NextTrack()
        {
            NextTrack(musicEnabled.Value);
        }

        private void NextTrack(bool isMusicEnabled)
        {
            if (music.Count > 0 && isMusicEnabled)
            {
                currentTrackIndex = shuffle ? Random.Range(0, music.Count) : (++currentTrackIndex % music.Count);
                Debug.AssertFormat(0 <= currentTrackIndex & currentTrackIndex < music.Count, "Invalid track index {0}", currentTrackIndex);
                audioSource.clip = music[currentTrackIndex];
                audioSource.Play();
            }
        }

        public void OnMusicEnabledChanged(bool isEnabled)
        {
            if (isEnabled)
            {
                NextTrack(isEnabled);
            }
            else
            {
                audioSource.Stop();
            }

            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).gameObject.SetActive(isEnabled);
            }
        }

        public void Play(AudioClip audioClip)
        {
            if (musicEnabled.Value && audioSource.clip != audioClip)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }

        public void PlayOneShot(AudioClip audioClip)
        {
            if (musicEnabled.Value)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }

        public void Stop()
        {
            audioSource.Stop();
        }

        #endregion
    }
}
