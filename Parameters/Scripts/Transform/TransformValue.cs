using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(TransformValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Transform/Transform Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class TransformValue : ParameterValue<Transform, TransformValueChangedEvent>
    {
    }
}
