using Celeste.Events;
using Celeste.Narrative.Backgrounds;
using UnityEngine;

namespace Celeste.Narrative.Events
{
    [AddComponentMenu("Celeste/Events/Background/Background Event Listener")]
    public class BackgroundEventListener : ParameterisedEventListener<Background, BackgroundEvent, BackgroundUnityEvent>
    {
    }
}
