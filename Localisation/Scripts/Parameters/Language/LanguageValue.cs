using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Localisation.Parameters
{
    [CreateAssetMenu(fileName = nameof(LanguageValue), menuName = "Celeste/Parameters/Localisation/Language Value")]
    public class LanguageValue : ParameterValue<Language, LanguageValueChangedEvent>
    {
        public string Localise(LocalisationKey localisationKey)
        {
            return Value.Localise(localisationKey);
        }
    }
}
