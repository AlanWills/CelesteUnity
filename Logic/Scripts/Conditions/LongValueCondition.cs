using Celeste.Parameters;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Logic
{
    [CreateAssetMenu(fileName = "LongValueCondition", menuName = "Celeste/Logic/Long Value Condition")]
    [DisplayName("Long")]
    public class LongValueCondition : ParameterizedValueCondition<long, LongValue, LongReference>
    {
        public override bool Check()
        {
            return value.Value.SatisfiesComparison(condition, target.Value);
        }
    }
}
