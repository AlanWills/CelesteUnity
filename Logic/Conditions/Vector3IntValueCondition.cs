using Celeste.Parameters;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Logic
{
    [CreateAssetMenu(fileName = "Vector3ValueCondition", menuName = "Celeste/Logic/Vector3 Value Condition")]
    [DisplayName("Vector3 Int")]
    public class Vector3IntValueCondition : ParameterizedValueCondition<Vector3Int, Vector3IntValue, Vector3IntReference>
    {
        public override bool Check()
        {
            return value.Value.SatisfiesEquality(condition, target.Value);
        }
    }
}
