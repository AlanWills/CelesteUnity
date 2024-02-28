using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "AudioClipReference", menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Audio/AudioClip Reference", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class AudioClipReference : ParameterReference<AudioClip, AudioClipValue, AudioClipReference>
    {
    }
}
