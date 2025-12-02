using Celeste.DeckBuilding.Cards;
using Celeste.Objects;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Catalogue
{
    [CreateAssetMenu(
        fileName = nameof(CardCatalogue), 
        menuName = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM + "Catalogue/Card Catalogue",
        order = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM_PRIORITY)]
    public class CardCatalogue : ListScriptableObject<Card>, IAutomaticImportSettings
    {
        public AutomaticImportBehaviour ImportBehaviour => importBehaviour;
        
        [SerializeField] private AutomaticImportBehaviour importBehaviour;
        
        public Card FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}