using Celeste.Events;
using Celeste.Sound.Settings;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(AudioClipSettingsValue), menuName = "Celeste/Parameters/Audio/Audio Clip Settings Value")]
    public class AudioClipSettingsValue : ParameterValue<AudioClipSettings, AudioClipSettingsValueChangedEvent>
    {
    }
}
