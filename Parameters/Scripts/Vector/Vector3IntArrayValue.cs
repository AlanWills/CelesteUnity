using Celeste.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(Vector3IntArrayValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Vector/Vector3Int Array Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class Vector3IntArrayValue : ParameterValue<List<Vector3Int>, Vector3IntArrayValueChangedEvent>
    {
    }
}
