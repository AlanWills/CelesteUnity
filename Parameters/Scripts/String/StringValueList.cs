using Celeste.Objects;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(StringValueList), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "String/String Value List", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class StringValueList : ListScriptableObject<StringValue>
    {
    }
}
