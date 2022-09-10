using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(GameObjectValue), menuName = "Celeste/Parameters/Game Object/GameObject Value")]
    public class GameObjectValue : ParameterValue<GameObject, GameObjectValueChangedEvent>
    {
    }
}
