using Celeste.Objects;
using UnityEngine;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(StringOptionList), menuName = "Celeste/Options/String/String Option List")]
    public class StringOptionList : ListScriptableObject<StringOption>
    {
    }
}
