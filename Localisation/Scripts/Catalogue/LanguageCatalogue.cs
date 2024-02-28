using Celeste.Objects;
using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Localisation.Catalogue
{
    [CreateAssetMenu(fileName = nameof(LanguageCatalogue), menuName = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM + "Language Catalogue", order = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM_PRIORITY)]
    public class LanguageCatalogue : ArrayScriptableObject<Language>
    {
        public Language FindLanguageForTwoLetterCountryCode(string countryCode)
        {
            return FindItem(x => StringComparer.OrdinalIgnoreCase.Compare(x.CountryCode, countryCode) == 0);
        }
    }
}