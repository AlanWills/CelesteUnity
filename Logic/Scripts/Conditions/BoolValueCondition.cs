using Celeste.Parameters;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Logic
{
    [CreateAssetMenu(fileName = "BoolValueCondition", menuName = CelesteMenuItemConstants.LOGIC_MENU_ITEM + "Bool Value Condition", order = CelesteMenuItemConstants.LOGIC_MENU_ITEM_PRIORITY)]
    [DisplayName("Bool")]
    public class BoolValueCondition : ParameterizedValueCondition<bool, BoolValue, BoolReference>
    {
        protected override bool DoCheck()
        {
            return value.Value.SatisfiesComparison(condition, target.Value);
        }
    }
}
