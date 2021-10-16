using Celeste.DataStructures;
using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Decks;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Shuffler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding
{
    [AddComponentMenu("Celeste/Deck Building/Draw Pile")]
    public class DrawPile : MonoBehaviour, ICardStorage
    {
        #region Properties and Fields

        public bool Empty
        {
            get { return cards.Count == 0; }
        }

        public int NumCards
        {
            get { return cards.Count; }
        }

        [SerializeField] private CardRuntimeEvent cardAddedEvent;
        [SerializeField] private CardRuntimeEvent cardRemovedEvent;

        private List<CardRuntime> cards = new List<CardRuntime>();

        #endregion

        public void AddCard(CardRuntime card)
        {
            UnityEngine.Debug.Assert(card != null, $"Inputting null card into {nameof(DrawPile.AddCard)}.");
            cards.Add(card);
            cardAddedEvent.Invoke(card);
        }

        public CardRuntime GetCard(int cardIndex)
        {
            return cards.Get(cardIndex);
        }

        public CardRuntime DrawCard()
        {
            UnityEngine.Debug.Assert(NumCards > 0, "No cards left in draw pile.");
            CardRuntime card = cards[NumCards - 1];
            cards.RemoveAt(NumCards - 1);
            cardRemovedEvent.Invoke(card);

            return card;
        }

        public void Shuffle(CardShuffler shuffler)
        {
            shuffler.Shuffle(cards);
        }
    }
}