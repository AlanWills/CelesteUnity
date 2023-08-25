using Celeste.Parameters;
using Celeste.Sound;
using UnityEngine;

namespace Celeste.FSM.Nodes.Events
{
    [CreateNodeMenu("Celeste/Events/Raisers/AudioClipEvent Raiser")]
    public class AudioClipEventRaiserNode : ParameterisedEventRaiserNode<AudioClip, AudioClipValue, AudioClipReference, AudioClipEvent>
    {
    }
}
