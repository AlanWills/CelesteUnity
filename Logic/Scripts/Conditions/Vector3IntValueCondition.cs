using Celeste.Parameters;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Logic
{
    [CreateAssetMenu(fileName = "Vector3ValueCondition", menuName = CelesteMenuItemConstants.LOGIC_MENU_ITEM + "Vector3 Value Condition", order = CelesteMenuItemConstants.LOGIC_MENU_ITEM_PRIORITY)]
    [DisplayName("Vector3 Int")]
    public class Vector3IntValueCondition : ParameterizedValueCondition<Vector3Int, Vector3IntValue, Vector3IntReference>
    {
        protected override bool DoCheck()
        {
            return value.Value.SatisfiesEquality(condition, target.Value);
        }
    }
}
