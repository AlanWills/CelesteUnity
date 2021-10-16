using Celeste.Narrative;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Story/Story Event Raiser")]
    public class StoryEventRaiser : ParameterisedEventRaiser<Story, StoryEvent>
    {
    }
}
