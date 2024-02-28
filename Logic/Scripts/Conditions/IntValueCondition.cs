using Celeste.Parameters;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Logic
{
    [CreateAssetMenu(fileName = "IntValueCondition", menuName = CelesteMenuItemConstants.LOGIC_MENU_ITEM + "Int Value Condition", order = CelesteMenuItemConstants.LOGIC_MENU_ITEM_PRIORITY)]
    [DisplayName("Int")]
    public class IntValueCondition : ParameterizedValueCondition<int, IntValue, IntReference>
    {
        protected override bool DoCheck()
        {
            return value.Value.SatisfiesComparison(condition, target.Value);
        }
    }
}
