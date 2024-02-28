using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(IntOption), menuName = CelesteMenuItemConstants.OPTIONS_MENU_ITEM + "Int/Int Option", order = CelesteMenuItemConstants.OPTIONS_MENU_ITEM_PRIORITY)]
    public class IntOption : Option<int, IntValue>
    {
    }
}
