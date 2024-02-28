using Celeste.Objects;
using UnityEditor;
using UnityEngine;

namespace Celeste.Narrative.Characters
{
    [CreateAssetMenu(fileName = nameof(CharacterCustomisationCatalogue), menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "Characters/Character Customisation Catalogue", order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class CharacterCustomisationCatalogue : ArrayScriptableObject<CharacterCustomisation>
    {
        public T FindByGuid<T>(int guid) where T : CharacterCustomisation
        {
            return FindItem(x => x is T && x.Guid == guid) as T;
        }
    }
}