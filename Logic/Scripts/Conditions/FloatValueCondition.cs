using Celeste.Parameters;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Logic
{
    [CreateAssetMenu(fileName = "FloatValueCondition", menuName = "Celeste/Logic/Float Value Condition")]
    [DisplayName("Float")]
    public class FloatValueCondition : ParameterizedValueCondition<float, FloatValue, FloatReference>
    {
        public override bool Check()
        {
            return value.Value.SatisfiesComparison(condition, target.Value);
        }
    }
}
