using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeste.Parameters.Input
{
    [CreateAssetMenu(fileName = nameof(KeyReference), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Input/Key Reference", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class KeyReference : ParameterReference<Key, KeyValue, KeyReference>
    {
    }
}
