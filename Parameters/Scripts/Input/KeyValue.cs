using Celeste.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeste.Parameters.Input
{
    [CreateAssetMenu(fileName = nameof(KeyValue), menuName = "Celeste/Parameters/Input/Key Value")]
    public class KeyValue : ParameterValue<Key, KeyValueChangedEvent>
    {
    }
}
