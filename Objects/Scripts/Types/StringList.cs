using UnityEngine;

namespace Celeste.Objects.Types
{
    [CreateAssetMenu(fileName = nameof(StringList), menuName = "Celeste/Objects/Lists/String List")]
    public class StringList : ListScriptableObject<string>
    {
    }
}
