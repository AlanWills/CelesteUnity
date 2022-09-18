using Celeste.Objects;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(StringValueList), menuName = "Celeste/Parameters/String/String Value List")]
    public class StringValueList : ListScriptableObject<StringValue>
    {
    }
}
