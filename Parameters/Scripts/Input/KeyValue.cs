using Celeste.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeste.Parameters.Input
{
    [CreateAssetMenu(fileName = nameof(KeyValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Input/Key Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class KeyValue : ParameterValue<Key, KeyValueChangedEvent>
    {
    }
}
