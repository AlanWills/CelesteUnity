using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(StringOption), menuName = CelesteMenuItemConstants.OPTIONS_MENU_ITEM + "String/String Option", order = CelesteMenuItemConstants.OPTIONS_MENU_ITEM_PRIORITY)]
    public class StringOption : Option<string, StringValue>
    {
    }
}
