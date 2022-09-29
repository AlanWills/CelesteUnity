using Celeste.Objects;
using UnityEngine;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(FloatOptionList), menuName = "Celeste/Options/Float/Float Option List")]
    public class FloatOptionList : ListScriptableObject<FloatOption>
    {
    }
}
