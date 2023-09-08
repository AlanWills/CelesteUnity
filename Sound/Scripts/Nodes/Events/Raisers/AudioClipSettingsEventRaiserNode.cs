using Celeste.Parameters;
using Celeste.Sound;
using Celeste.Sound.Settings;
using UnityEngine;

namespace Celeste.FSM.Nodes.Events
{
    [CreateNodeMenu("Celeste/Events/Raisers/AudioClipSettingsEvent Raiser")]
    public class AudioClipSettingsEventRaiserNode : ParameterisedEventRaiserNode<AudioClipSettings, AudioClipSettingsValue, AudioClipSettingsReference, AudioClipSettingsEvent>
    {
    }
}
