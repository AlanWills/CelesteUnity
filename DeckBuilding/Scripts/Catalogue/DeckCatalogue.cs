using Celeste.DeckBuilding.Decks;
using Celeste.Objects;
using UnityEngine;

namespace Celeste.DeckBuilding.Catalogue
{
    [CreateAssetMenu(
        fileName = nameof(DeckCatalogue), 
        menuName = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM + "Catalogue/Deck Catalogue",
        order = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM_PRIORITY)]
    public class DeckCatalogue : ListScriptableObject<Deck>
    {
        public Deck FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}
