using Celeste.Objects;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(IntValueList), menuName = "Celeste/Parameters/Numeric/Int/Int Value List")]
    public class IntValueList : ListScriptableObject<IntValue>
    {
    }
}
