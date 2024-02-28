using Celeste.Events;
using Celeste.Sound.Settings;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(AudioClipSettingsValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Audio/Audio Clip Settings Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class AudioClipSettingsValue : ParameterValue<AudioClipSettings, AudioClipSettingsValueChangedEvent>
    {
    }
}
