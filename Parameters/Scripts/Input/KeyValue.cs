using Celeste.Events;
using UnityEngine;
#if USE_NEW_INPUT_SYSTEM
using Key = UnityEngine.InputSystem.Key;
#else
using Key = UnityEngine.KeyCode;
#endif

namespace Celeste.Parameters.Input
{
    [CreateAssetMenu(fileName = nameof(KeyValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Input/Key Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class KeyValue : ParameterValue<Key, KeyValueChangedEvent>
    {
    }
}