using Celeste.Assets;
using Celeste.Parameters;
using Celeste.Sound.Settings;
using Celeste.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Sound
{
    [AddComponentMenu("Celeste/Sound/Music Manager")]
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour, IHasAssets
    {
        #region Properties and Fields

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private MusicSettings musicSettings;

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
            this.TryGet(ref audioSource);
        }

        #endregion

        #region IHasAssets

        public bool ShouldLoadAssets()
        {
            return musicSettings.ShouldLoadAssets();
        }

        public IEnumerator LoadAssets()
        {
            yield return musicSettings.LoadAssets();

            musicSettings.AddOnMusicEnabledChangedCallback(OnMusicEnabledChanged);
            musicSettings.AddOnPlayMusicCallback(Play);
            musicSettings.AddOnPlayMusicOneShotCallback(PlayOneShot);
        }

        #endregion

        #region Utility

        public void NextTrack()
        {
            NextTrack(musicSettings.Enabled);
        }

        private void NextTrack(bool isMusicEnabled)
        {
            var tracks = musicSettings.Tracks;

            if (tracks.Count > 0 && isMusicEnabled)
            {
                currentTrackIndex = musicSettings.Shuffle ? Random.Range(0, tracks.Count) : (++currentTrackIndex % tracks.Count);
                Debug.Assert(0 <= currentTrackIndex & currentTrackIndex < tracks.Count, $"Invalid track index {currentTrackIndex}");
                audioSource.clip = tracks[currentTrackIndex];
                audioSource.Play();
            }
        }

        private void OnMusicEnabledChanged(bool isEnabled)
        {
            if (isEnabled)
            {
                NextTrack(isEnabled);
            }
            else
            {
                Stop();
            }
        }

        private void Play(AudioClip audioClip)
        {
            if (musicSettings.Enabled && audioSource.clip != audioClip)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }

        private void PlayOneShot(AudioClip audioClip)
        {
            if (musicSettings.Enabled)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }

        private void Stop()
        {
            audioSource.Stop();
        }

        #endregion
    }
}
