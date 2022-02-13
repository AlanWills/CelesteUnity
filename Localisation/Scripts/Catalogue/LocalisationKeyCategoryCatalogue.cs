using Celeste.Objects;
using System.Collections;
using UnityEngine;

namespace Celeste.Localisation.Catalogue
{
    [CreateAssetMenu(fileName = nameof(LocalisationKeyCategoryCatalogue), menuName = "Celeste/Localisation/Localisation Key Category Catalogue")]
    public class LocalisationKeyCategoryCatalogue : ArrayScriptableObject<LocalisationKeyCategory>
    {
    }
}