using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "AudioClipValue", menuName = "Celeste/Parameters/Audio/AudioClip Value")]
    public class AudioClipValue : ParameterValue<AudioClip>
    {
        [SerializeField] private AudioClipEvent onValueChanged;
        protected override ParameterisedEvent<AudioClip> OnValueChanged => onValueChanged;
    }
}
