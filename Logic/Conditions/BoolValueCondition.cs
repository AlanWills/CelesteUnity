using Celeste.Parameters;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Logic
{
    [CreateAssetMenu(fileName = "BoolValueCondition", menuName = "Celeste/Logic/Bool Value Condition")]
    [DisplayName("Bool")]
    public class BoolValueCondition : ParameterizedValueCondition<bool, BoolValue, BoolReference>
    {
        public override bool Check()
        {
            return value.Value.SatisfiesComparison(condition, target.Value);
        }
    }
}
