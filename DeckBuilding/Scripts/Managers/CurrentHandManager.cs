using Celeste.DeckBuilding.Events;
using UnityEngine;

namespace Celeste.DeckBuilding.Managers
{
    [AddComponentMenu("Celeste/Deck Building/Managers/Current Hand Manager")]
    public class CurrentHandManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private CurrentHand currentHand;

        #endregion

        public void OnDrawCardsFromDeck(DrawCardsFromDeckArgs args)
        {
            for (int i = 0; i < args.quantity; ++i)
            {
                CardRuntime card = args.deck.DrawCard();
                currentHand.AddCard(card);
            }
        }
    }
}
