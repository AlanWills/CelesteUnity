using Celeste.Objects;
using UnityEngine;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(BoolOptionList), menuName = "Celeste/Options/Bool/Bool Option List")]
    public class BoolOptionList : ListScriptableObject<BoolOption>
    {
    }
}
