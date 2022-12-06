using Celeste.DeckBuilding.Decks;
using Celeste.Objects;
using UnityEngine;

namespace Celeste.DeckBuilding.Catalogue
{
    [CreateAssetMenu(fileName = nameof(DeckCatalogue), menuName = "Celeste/Deck Building/Catalogue/Deck Catalogue")]
    public class DeckCatalogue : ListScriptableObject<Deck>
    {
        public Deck FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}
