using Celeste.Objects;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Cards
{
    [CreateAssetMenu(fileName = nameof(CardCatalogue), menuName = "Celeste/Deck Building/Cards/Card Catalogue")]
    public class CardCatalogue : ArrayScriptableObject<Card>
    {
        public Card FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}