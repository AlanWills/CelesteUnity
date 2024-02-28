using Celeste.Sound.Settings;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(AudioClipReference), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Audio/Audio Clip Settings Reference", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class AudioClipSettingsReference : ParameterReference<AudioClipSettings, AudioClipSettingsValue, AudioClipSettingsReference>
    {
    }
}
