using Celeste.Objects;
using UnityEngine;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(BoolOptionList), menuName = CelesteMenuItemConstants.OPTIONS_MENU_ITEM + "Bool/Bool Option List", order = CelesteMenuItemConstants.OPTIONS_MENU_ITEM_PRIORITY)]
    public class BoolOptionList : ListScriptableObject<BoolOption>
    {
    }
}
