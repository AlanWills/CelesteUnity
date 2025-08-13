using Celeste.Events;
using UnityEngine;

namespace Celeste.Narrative.Events
{
    [AddComponentMenu("Celeste/Events/Background/Set Background Event Listener")]
    public class SetBackgroundEventListener : ParameterisedEventListener<SetBackgroundEventArgs, SetBackgroundEvent, SetBackgroundUnityEvent>
    {
    }
}
