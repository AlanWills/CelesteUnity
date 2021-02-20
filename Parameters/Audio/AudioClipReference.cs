using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "AudioClipReference", menuName = "Celeste/Parameters/Audio/AudioClip Reference")]
    public class AudioClipReference : ParameterReference<AudioClip, AudioClipValue, AudioClipReference>
    {
    }
}
