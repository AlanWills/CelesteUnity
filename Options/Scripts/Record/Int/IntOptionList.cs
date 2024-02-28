using Celeste.Objects;
using UnityEngine;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(IntOptionList), menuName = CelesteMenuItemConstants.OPTIONS_MENU_ITEM + "Int/Int Option List", order = CelesteMenuItemConstants.OPTIONS_MENU_ITEM_PRIORITY)]
    public class IntOptionList : ListScriptableObject<IntOption>
    {
    }
}
