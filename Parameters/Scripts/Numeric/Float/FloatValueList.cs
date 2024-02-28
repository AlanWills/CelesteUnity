using Celeste.Objects;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(FloatValueList), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Numeric/Float/Float Value List", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class FloatValueList : ListScriptableObject<FloatValue>
    {
    }
}
