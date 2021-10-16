using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Results;
using Celeste.DeckBuilding.Shuffler;
using UnityEngine;

namespace Celeste.DeckBuilding.Decks
{
    [CreateAssetMenu(fileName = nameof(PrebuiltDeck), menuName = "Celeste/Deck Building/Decks/Prebuilt Deck")]
    public class PrebuiltDeck : ScriptableObject
    {
        #region Properties and Fields

        [SerializeField] private CardShuffler cardShuffler;
        [SerializeField] private Card[] cards;

        #endregion

        public Deck ToDeck()
        {
            Deck deck = new Deck(cardShuffler);
            for (int i = 0, n = cards.Length; i < n; ++i)
            {
                deck.AddCard(cards[i]);
            }

            return deck;
        }
    }
}