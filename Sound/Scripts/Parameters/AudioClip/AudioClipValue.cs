using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(AudioClipValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Audio/AudioClip Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class AudioClipValue : ParameterValue<AudioClip, AudioClipValueChangedEvent>
    {
    }
}
