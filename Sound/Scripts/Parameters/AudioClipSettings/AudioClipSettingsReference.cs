using Celeste.Sound.Settings;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(AudioClipReference), menuName = "Celeste/Parameters/Audio/Audio Clip Settings Reference")]
    public class AudioClipSettingsReference : ParameterReference<AudioClipSettings, AudioClipSettingsValue, AudioClipSettingsReference>
    {
    }
}
