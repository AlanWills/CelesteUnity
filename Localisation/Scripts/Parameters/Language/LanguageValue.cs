using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Localisation.Parameters
{
    [CreateAssetMenu(fileName = nameof(LanguageValue), menuName = "Celeste/Parameters/Localisation/Language Value")]
    public class LanguageValue : ParameterValue<Language>
    {
        [SerializeField] private LanguageEvent onValueChanged;
        protected override ParameterisedEvent<Language> OnValueChanged => onValueChanged;
    }
}
