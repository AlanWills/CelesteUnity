using Celeste.Events;
using Celeste.Twine;
using UnityEngine;

namespace Celeste.Narrative.Events
{
    [AddComponentMenu("Celeste/Events/Twine/Twine Node Event Listener")]
    public class TwineNodeEventListener : ParameterisedEventListener<TwineNode, TwineNodeEvent, TwineNodeUnityEvent>
    {
    }
}
