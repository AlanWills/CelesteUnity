using UnityEngine;

namespace Celeste.Events
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
    }
}
