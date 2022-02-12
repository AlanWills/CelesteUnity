using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Localisation.Parameters
{
    [CreateAssetMenu(fileName = nameof(LocalisationKeyCategoryReference), menuName = "Celeste/Parameters/Localisation/Localisation Key Category Reference")]
    public class LocalisationKeyCategoryReference : ParameterReference<LocalisationKeyCategory, LocalisationKeyCategoryValue, LocalisationKeyCategoryReference>
    {
    }
}
