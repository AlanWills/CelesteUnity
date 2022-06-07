using Celeste.Parameters;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Logic
{
    [CreateAssetMenu(fileName = "BoolValueCondition", menuName = "Celeste/Logic/Bool Value Condition")]
    [DisplayName("Bool")]
    public class BoolValueCondition : ParameterizedValueCondition<bool, BoolValue, BoolReference>
    {
        protected override bool DoCheck()
        {
            return value.Value.SatisfiesComparison(condition, target.Value);
        }
    }
}
