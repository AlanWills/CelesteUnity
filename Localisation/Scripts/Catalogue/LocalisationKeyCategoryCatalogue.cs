using Celeste.Objects;
using System.Collections;
using UnityEngine;

namespace Celeste.Localisation.Catalogue
{
    [CreateAssetMenu(fileName = nameof(LocalisationKeyCategoryCatalogue), menuName = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM + "Localisation Key Category Catalogue", order = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM_PRIORITY)]
    public class LocalisationKeyCategoryCatalogue : ListScriptableObject<LocalisationKeyCategory>
    {
        public LocalisationKeyCategory FindByCategoryName(string name)
        {
            return FindItem(x => string.CompareOrdinal(name, x.CategoryName) == 0);
        }
    }
}