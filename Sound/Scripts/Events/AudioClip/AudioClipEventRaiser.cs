using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Sound
{
    [AddComponentMenu("Celeste/Events/Audio/Audio Clip Event Raiser")]
    public class AudioClipEventRaiser : ParameterisedEventRaiser<AudioClip, AudioClipEvent>
    {
        #region Properties and Fields

        [SerializeField] private AudioClip presetAudioClip;

        #endregion

        public void RaiseWithPreset()
        {
            Raise(presetAudioClip);
        }

        public void Raise(AudioClipValue audioClipValue)
        {
            Raise(audioClipValue.Value);
        }
    }
}
