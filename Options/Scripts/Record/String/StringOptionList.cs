using Celeste.Objects;
using UnityEngine;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(StringOptionList), menuName = CelesteMenuItemConstants.OPTIONS_MENU_ITEM + "String/String Option List", order = CelesteMenuItemConstants.OPTIONS_MENU_ITEM_PRIORITY)]
    public class StringOptionList : ListScriptableObject<StringOption>
    {
    }
}
