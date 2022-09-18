using Celeste.Objects;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(BoolValueList), menuName = "Celeste/Parameters/Bool/Bool Value List")]
    public class BoolValueList : ListScriptableObject<BoolValue>
    {
    }
}
