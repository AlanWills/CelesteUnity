using Celeste.Objects;
using UnityEngine;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(FloatOptionList), menuName = CelesteMenuItemConstants.OPTIONS_MENU_ITEM + "Float/Float Option List", order = CelesteMenuItemConstants.OPTIONS_MENU_ITEM_PRIORITY)]
    public class FloatOptionList : ListScriptableObject<FloatOption>
    {
    }
}
