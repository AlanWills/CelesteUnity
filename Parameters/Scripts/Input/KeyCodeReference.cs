using UnityEngine;

namespace Celeste.Parameters.Input
{
    [CreateAssetMenu(fileName = "New KeyCodeReference", menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Input/KeyCode Reference", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class KeyCodeReference : ParameterReference<KeyCode, KeyCodeValue, KeyCodeReference>
    {
    }
}
