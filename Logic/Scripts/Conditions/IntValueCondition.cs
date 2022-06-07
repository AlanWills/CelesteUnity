using Celeste.Parameters;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Logic
{
    [CreateAssetMenu(fileName = "IntValueCondition", menuName = "Celeste/Logic/Int Value Condition")]
    [DisplayName("Int")]
    public class IntValueCondition : ParameterizedValueCondition<int, IntValue, IntReference>
    {
        protected override bool DoCheck()
        {
            return value.Value.SatisfiesComparison(condition, target.Value);
        }
    }
}
