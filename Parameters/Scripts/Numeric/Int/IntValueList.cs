using Celeste.Objects;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(IntValueList), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Numeric/Int/Int Value List", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class IntValueList : ListScriptableObject<IntValue>
    {
    }
}
