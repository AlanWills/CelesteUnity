using Celeste.Parameters;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Logic
{
    [CreateAssetMenu(fileName = "StringValueCondition", menuName = "Celeste/Logic/String Value Condition")]
    [DisplayName("String")]
    public class StringValueCondition : ParameterizedValueCondition<string, StringValue, StringReference>
    {
        public override bool Check()
        {
            return value.Value.SatisfiesComparison(condition, target.Value);
        }
    }
}
