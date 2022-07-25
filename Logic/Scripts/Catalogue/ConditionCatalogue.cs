using Celeste.Objects;
using UnityEngine;

namespace Celeste.Logic.Catalogue
{
    [CreateAssetMenu(fileName = nameof(ConditionCatalogue), menuName = "Celeste/Logic/Condition Catalogue")]
    public class ConditionCatalogue : ListScriptableObject<Condition>
    {
    }
}
