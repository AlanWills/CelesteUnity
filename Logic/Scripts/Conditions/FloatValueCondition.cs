using Celeste.Parameters;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Logic
{
    [CreateAssetMenu(fileName = "FloatValueCondition", menuName = CelesteMenuItemConstants.LOGIC_MENU_ITEM + "Float Value Condition", order = CelesteMenuItemConstants.LOGIC_MENU_ITEM_PRIORITY)]
    [DisplayName("Float")]
    public class FloatValueCondition : ParameterizedValueCondition<float, FloatValue, FloatReference>
    {
        protected override bool DoCheck()
        {
            return value.Value.SatisfiesComparison(condition, target.Value);
        }
    }
}
