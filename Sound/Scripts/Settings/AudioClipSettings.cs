using UnityEngine;

namespace Celeste.Sound.Settings
{
    [CreateAssetMenu(fileName = nameof(AudioClipSettings), menuName = "Celeste/Sound/Audio Clip Settings")]
    public class AudioClipSettings : ScriptableObject
    {
        #region Properties and Fields

        public AudioClip Clip => clip;
        public float Volume => volume * (audioChannelSettings != null ? audioChannelSettings.Volume : 0f);

        [SerializeField] private AudioClip clip;
        [SerializeField, Tools.Attributes.GUI.Range(0f, 1f)] private float volume = 1;
        [SerializeField] private AudioChannelSettings audioChannelSettings;

        #endregion

        public static AudioClipSettings Create(AudioClip audioClip, AudioChannelSettings audioChannelSettings)
        {
            AudioClipSettings audioClipSettings = CreateInstance<AudioClipSettings>();
            audioClipSettings.name = audioClip.name;
            audioClipSettings.clip = audioClip;
            audioClipSettings.audioChannelSettings = audioChannelSettings;

            return audioClipSettings;
        }
    }
}
