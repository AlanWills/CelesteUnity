using Celeste.Parameters;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Logic
{
    [CreateAssetMenu(fileName = "UIntValueCondition", menuName = "Celeste/Logic/UInt Value Condition")]
    [DisplayName("UInt")]
    public class UIntValueCondition : ParameterizedValueCondition<uint, UIntValue, UIntReference>
    {
        protected override bool DoCheck()
        {
            return value.Value.SatisfiesComparison(condition, target.Value);
        }
    }
}
