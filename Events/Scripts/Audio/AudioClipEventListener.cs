using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Audio/Audio Clip Event Listener")]
    public class AudioClipEventListener : ParameterisedEventListener<AudioClip, AudioClipEvent, AudioClipUnityEvent>
    {
    }
}
