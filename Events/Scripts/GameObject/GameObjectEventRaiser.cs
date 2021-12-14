using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Game Object Event Raiser")]
    public class GameObjectEventRaiser : ParameterisedEventRaiser<GameObject, GameObjectEvent>
    {
    }
}
