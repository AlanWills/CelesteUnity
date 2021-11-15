using Celeste.Events;
using Celeste.Twine;
using UnityEngine;

namespace Celeste.Narrative.Events
{
    [AddComponentMenu("Celeste/Events/Twine/Twine Story Event Listener")]
    public class TwineStoryEventListener : ParameterisedEventListener<TwineStory, TwineStoryEvent, TwineStoryUnityEvent>
    {
    }
}
