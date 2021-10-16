using Celeste.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Narrative.Events
{
    [AddComponentMenu("Celeste/Events/Story/Story Event Listener")]
    public class StoryEventListener : ParameterisedEventListener<Story, StoryEvent, StoryUnityEvent>
    {
    }
}
