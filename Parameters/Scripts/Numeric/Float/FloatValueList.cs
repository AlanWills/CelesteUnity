using Celeste.Objects;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(FloatValueList), menuName = "Celeste/Parameters/Numeric/Float/Float Value List")]
    public class FloatValueList : ListScriptableObject<FloatValue>
    {
    }
}
