using Celeste.Narrative.Backgrounds;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Background/Background Event Raiser")]
    public class BackgroundEventRaiser : ParameterisedEventRaiser<Background, BackgroundEvent>
    {
    }
}
