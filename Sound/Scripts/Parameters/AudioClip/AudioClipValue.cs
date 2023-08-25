using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(AudioClipValue), menuName = "Celeste/Parameters/Audio/AudioClip Value")]
    public class AudioClipValue : ParameterValue<AudioClip, AudioClipValueChangedEvent>
    {
    }
}
