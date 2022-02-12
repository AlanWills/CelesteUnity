using Celeste.Objects;
using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Localisation.Catalogue
{
    [CreateAssetMenu(fileName = nameof(LanguageCatalogue), menuName = "Celeste/Localisation/Language Catalogue")]
    public class LanguageCatalogue : ArrayScriptableObject<Language>
    {
        public Language FindLanguageForTwoLetterCountryCode(string countryCode)
        {
            return FindItem(x => StringComparer.OrdinalIgnoreCase.Compare(x.CountryCode, countryCode) == 0);
        }
    }
}