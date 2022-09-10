using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters.Input
{
    [CreateAssetMenu(fileName = nameof(KeyCodeValue), menuName = "Celeste/Parameters/Input/KeyCode Value")]
    public class KeyCodeValue :  ParameterValue<KeyCode, KeyCodeValueChangedEvent>
    {
    }
}
