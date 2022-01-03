using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Localisation.Parameters
{
    [CreateAssetMenu(fileName = nameof(LanguageReference), menuName = "Celeste/Parameters/Localisation/Language Reference")]
    public class LanguageReference : ParameterReference<Language, LanguageValue, LanguageReference>
    {
    }
}
