using Celeste.Assets;
using Celeste.Events;
using Celeste.Sound.Settings;
using Celeste.Tools;
using System.Collections;
using UnityEngine;

namespace Celeste.Sound
{
    [AddComponentMenu("Celeste/Sound/Music Manager")]
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour, IHasAssets, IMusicListener
    {
        #region Properties and Fields

        public MusicSettings MusicSettings
        {
            get => musicSettings;
            set
            {
                musicSettings = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private MusicSettings musicSettings;

        private int currentTrackIndex = -1;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if (audioSource.playOnAwake)
            {
                NextTrack();
            }
        }

        private void OnDisable()
        {
            musicSettings.ShutdownListener(this);
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

            musicSettings.SetupListener(this);
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

        public void OnMusicEnabledChanged(ValueChangedArgs<bool> args)
        {
            if (args.newValue)
            {
                NextTrack(args.newValue);
            }
            else
            {
                Stop();
            }
        }

        public void Play(AudioClip audioClip)
        {
            if (musicSettings.Enabled && audioSource.clip != audioClip)
            {
                audioSource.volume = 1.0f;
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }

        public void Play(AudioClipSettings audioClipSettings)
        {
            if (musicSettings.Enabled && audioSource.clip != audioClipSettings.Clip)
            {
                audioSource.volume = audioClipSettings.Volume;
                audioSource.clip = audioClipSettings.Clip;
                audioSource.Play();
            }
        }

        public void PlayOneShot(AudioClip audioClip)
        {
            if (musicSettings.Enabled)
            {
                audioSource.volume = 1.0f;
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }

        public void PlayOneShot(AudioClipSettings audioClipSettings)
        {
            if (musicSettings.Enabled)
            {
                audioSource.volume = audioClipSettings.Volume;
                audioSource.clip = audioClipSettings.Clip;
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
