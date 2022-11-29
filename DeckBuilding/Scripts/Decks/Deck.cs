using Celeste.DataStructures;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Shuffler;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding.Decks
{
    [CreateAssetMenu(fileName = nameof(Deck), menuName = "Celeste/Deck Building/Deck")]
    public class Deck : ScriptableObject
    {
        #region Properties and Fields

        public int NumCardsInDrawPile => drawPile.Count;
        public int NumCardsInDiscardPile => discardPile.Count;
        public int NumCardsInRemovedPile => removedPile.Count;
        public bool DrawPileEmpty => NumCardsInDrawPile == 0;

        public CardShuffler CardShuffler
        {
            get { return cardShuffler; }
            set { cardShuffler = value; }
        }

        [SerializeField] private CardShuffler cardShuffler;

        [Header("Draw Pile Events")]
        [SerializeField] private CardRuntimeEvent cardAddedToDrawPileEvent;
        [SerializeField] private CardRuntimeEvent cardRemovedFromDrawPileEvent;

        [Header("Discard Pile Events")]
        [SerializeField] private CardRuntimeEvent cardAddedToDiscardPileEvent;
        [SerializeField] private CardRuntimeEvent cardRemovedFromDiscardPileEvent;

        [Header("Removed Pile Events")]
        [SerializeField] private CardRuntimeEvent cardAddedToRemovedPileEvent;
        [SerializeField] private CardRuntimeEvent cardRemovedFromRemovedPileEvent;

        [NonSerialized] private List<CardRuntime> drawPile = new List<CardRuntime>();
        [NonSerialized] private List<CardRuntime> discardPile = new List<CardRuntime>();
        [NonSerialized] private List<CardRuntime> removedPile = new List<CardRuntime>();

        #endregion

        #region Draw Pile Management

        public void AddCardToDrawPile(CardRuntime cardRuntime)
        {
            drawPile.Add(cardRuntime);
            cardAddedToDrawPileEvent.Invoke(cardRuntime);
        }

        public CardRuntime DrawCard()
        {
            UnityEngine.Debug.Assert(NumCardsInDrawPile > 0, "No cards left in draw pile.");
            CardRuntime card = drawPile[NumCardsInDrawPile - 1];
            drawPile.RemoveAt(NumCardsInDrawPile - 1);
            cardRemovedFromDrawPileEvent.Invoke(card);

            return card;
        }

        public void RemakeDrawPile()
        {
            for (int i = NumCardsInDiscardPile - 1; i >= 0; --i)
            {
                CardRuntime card = RemoveCardFromDiscardPile(i);
                AddCardToDrawPile(card);
            }
        }

        public void ShuffleDrawPile()
        {
            cardShuffler.Shuffle(drawPile);
        }

        public CardRuntime GetCardInDrawPile(int index)
        {
            return drawPile.Get(index);
        }

        #endregion

        #region Discard Pile Management

        public void AddCardToDiscardPile(CardRuntime cardRuntime)
        {
            discardPile.Add(cardRuntime);
            cardAddedToDiscardPileEvent.Invoke(cardRuntime);
        }

        public CardRuntime RemoveCardFromDiscardPile(int index)
        {
            CardRuntime card = discardPile.Get(index);
            cardRemovedFromDiscardPileEvent.Invoke(card);
            discardPile.RemoveAt(index);

            return card;
        }

        public CardRuntime GetCardInDiscardPile(int index)
        {
            return discardPile.Get(index);
        }

        #endregion

        #region Removed Pile Management

        public void AddCardToRemovedPile(CardRuntime cardRuntime)
        {
            removedPile.Add(cardRuntime);
            cardAddedToRemovedPileEvent.Invoke(cardRuntime);
        }

        public CardRuntime GetCardInRemovedPile(int index)
        {
            return removedPile.Get(index);
        }

        #endregion
    }
}