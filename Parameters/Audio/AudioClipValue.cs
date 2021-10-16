using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "AudioClipValue", menuName = "Celeste/Parameters/Audio/AudioClip Value")]
    public class AudioClipValue : ParameterValue<AudioClip>
    {
    }
}
