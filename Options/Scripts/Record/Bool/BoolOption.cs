using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(BoolOption), menuName = CelesteMenuItemConstants.OPTIONS_MENU_ITEM + "Bool/Bool Option", order = CelesteMenuItemConstants.OPTIONS_MENU_ITEM_PRIORITY)]
    public class BoolOption : Option<bool, BoolValue>
    {
    }
}
