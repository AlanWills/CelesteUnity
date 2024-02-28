using Celeste.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Localisation.Catalogue
{
    [CreateAssetMenu(fileName = nameof(LocalisationKeyCatalogue), menuName = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM + "Localisation Key Catalogue", order = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM_PRIORITY)]
    public class LocalisationKeyCatalogue : DictionaryScriptableObject<string, LocalisationKey>
    {
    }
}