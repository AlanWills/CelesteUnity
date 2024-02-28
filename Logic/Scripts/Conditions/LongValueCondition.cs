using Celeste.Parameters;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Logic
{
    [CreateAssetMenu(fileName = "LongValueCondition", menuName = CelesteMenuItemConstants.LOGIC_MENU_ITEM + "Long Value Condition", order = CelesteMenuItemConstants.LOGIC_MENU_ITEM_PRIORITY)]
    [DisplayName("Long")]
    public class LongValueCondition : ParameterizedValueCondition<long, LongValue, LongReference>
    {
        protected override bool DoCheck()
        {
            return value.Value.SatisfiesComparison(condition, target.Value);
        }
    }
}
