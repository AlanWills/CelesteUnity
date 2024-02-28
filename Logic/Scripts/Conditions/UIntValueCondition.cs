using Celeste.Parameters;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Logic
{
    [CreateAssetMenu(fileName = "UIntValueCondition", menuName = CelesteMenuItemConstants.LOGIC_MENU_ITEM + "UInt Value Condition", order = CelesteMenuItemConstants.LOGIC_MENU_ITEM_PRIORITY)]
    [DisplayName("UInt")]
    public class UIntValueCondition : ParameterizedValueCondition<uint, UIntValue, UIntReference>
    {
        protected override bool DoCheck()
        {
            return value.Value.SatisfiesComparison(condition, target.Value);
        }
    }
}
