using Celeste.Objects;
using UnityEngine;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(IntOptionList), menuName = "Celeste/Options/Int/Int Option List")]
    public class IntOptionList : ListScriptableObject<IntOption>
    {
    }
}
