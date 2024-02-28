using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Localisation.Parameters
{
    [CreateAssetMenu(fileName = nameof(LanguageValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Localisation/Language Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class LanguageValue : ParameterValue<Language, LanguageValueChangedEvent>
    {
        public string Localise(LocalisationKey localisationKey)
        {
            return Value.Localise(localisationKey);
        }

        public string Truncate(int number)
        {
            return Value.Truncate(number);
        }
    }
}
