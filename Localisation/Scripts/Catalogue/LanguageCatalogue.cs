using Celeste.Objects;
using System.Collections;
using UnityEngine;

namespace Celeste.Localisation.Catalogue
{
    [CreateAssetMenu(fileName = nameof(LanguageCatalogue), menuName = "Celeste/Localisation/Language Catalogue")]
    public class LanguageCatalogue : ArrayScriptableObject<Language>
    {
        public Language FindLanguageForTwoLetterCountryCode(string countryCode)
        {
            return FindItem(x => string.CompareOrdinal(x.CountryCode, countryCode) == 0);
        }
    }
}