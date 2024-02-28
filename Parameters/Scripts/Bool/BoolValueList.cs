using Celeste.Objects;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(BoolValueList), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Bool/Bool Value List", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class BoolValueList : ListScriptableObject<BoolValue>
    {
    }
}
