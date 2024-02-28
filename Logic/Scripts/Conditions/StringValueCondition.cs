using Celeste.Parameters;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Logic
{
    [CreateAssetMenu(fileName = "StringValueCondition", menuName = CelesteMenuItemConstants.LOGIC_MENU_ITEM + "String Value Condition", order = CelesteMenuItemConstants.LOGIC_MENU_ITEM_PRIORITY)]
    [DisplayName("String")]
    public class StringValueCondition : ParameterizedValueCondition<string, StringValue, StringReference>
    {
        protected override bool DoCheck()
        {
            return value.Value.SatisfiesComparison(condition, target.Value);
        }
    }
}
