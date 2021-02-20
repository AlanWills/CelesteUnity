using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Audio/Audio Clip Event Raiser")]
    public class AudioClipEventRaiser : ParameterisedEventRaiser<AudioClip, AudioClipEvent>, ISupportsValueArgument<AudioClip, AudioClipValue>
    {
        public void Raise(AudioClipValue argument)
        {
            Raise(argument.Value);
        }
    }
}
