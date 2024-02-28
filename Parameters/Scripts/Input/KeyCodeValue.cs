using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters.Input
{
    [CreateAssetMenu(fileName = nameof(KeyCodeValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Input/KeyCode Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class KeyCodeValue :  ParameterValue<KeyCode, KeyCodeValueChangedEvent>
    {
    }
}
