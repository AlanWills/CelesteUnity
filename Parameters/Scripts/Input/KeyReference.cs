using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeste.Parameters.Input
{
    [CreateAssetMenu(fileName = nameof(KeyReference), menuName = "Celeste/Parameters/Input/Key Reference")]
    public class KeyReference : ParameterReference<Key, KeyValue, KeyReference>
    {
    }
}
