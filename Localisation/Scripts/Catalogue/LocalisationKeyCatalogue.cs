using Celeste.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Localisation.Catalogue
{
    [CreateAssetMenu(fileName = nameof(LocalisationKeyCatalogue), menuName = "Celeste/Localisation/Localisation Key Catalogue")]
    public class LocalisationKeyCatalogue : DictionaryScriptableObject<string, LocalisationKey>
    {
    }
}