using Celeste.DataStructures;
using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Shuffler;
using Celeste.Events;
using Celeste.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Decks
{
    [CreateAssetMenu(
        fileName = nameof(Deck), 
        menuName = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM + "Deck",
        order = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM_PRIORITY)]
    public class Deck : ScriptableObject, IIntGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get { return guid; }
            set
            {
                guid = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public string DisplayName => displayName;

        public int NumCardsInDrawPile => drawPile.Count;
        public int NumCardsInDiscardPile => discardPile.Count;
        public int NumCardsInRemovedPile => removedPile.Count;
        public bool DrawPileEmpty => NumCardsInDrawPile == 0;
        public bool DiscardPileEmpty => NumCardsInDiscardPile == 0;
        public bool RemovedPileEmpty => NumCardsInRemovedPile == 0;

        [SerializeField] private int guid;
        [SerializeField] private string displayName;
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

        public void AddCardToDrawPile(Card card)
        {
            AddCardToDrawPile(new CardRuntime(this, card));
        }

        public void AddCardToDrawPile(CardRuntime cardRuntime)
        {
            drawPile.Add(cardRuntime);
            cardAddedToDrawPileEvent.Invoke(cardRuntime);
        }

        public CardRuntime RemoveCardFromDrawPile(int index)
        {
            CardRuntime card = drawPile.Get(index);
            cardRemovedFromDrawPileEvent.Invoke(card);
            drawPile.RemoveAt(index);

            return card;
        }

        public CardRuntime DrawCard()
        {
            if (drawPile.Count + discardPile.Count == 0)
            {
                UnityEngine.Debug.LogAssertion($"No cards left in deck.");
                return null;
            }

            if (DrawPileEmpty)
            {
                RemakeDrawPile();
            }

            UnityEngine.Debug.Assert(NumCardsInDrawPile > 0, "No cards left in draw pile.");
            CardRuntime card = drawPile[NumCardsInDrawPile - 1];
            drawPile.RemoveAt(NumCardsInDrawPile - 1);
            cardRemovedFromDrawPileEvent.Invoke(card);

            return card;
        }

        private void RemakeDrawPile()
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

        public CardRuntime PeekTopCardOfDrawPile()
        {
            UnityEngine.Debug.Assert(!DrawPileEmpty, $"Attempting to peek top card of draw pile when it is empty.");
            return NumCardsInDrawPile > 0 ? drawPile[NumCardsInDrawPile - 1] : null;
        }

        #endregion

        #region Discard Pile Management

        public void AddCardToDiscardPile(Card card)
        {
            AddCardToDiscardPile(new CardRuntime(this, card));
        }

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

        public CardRuntime PeekTopCardOfDiscardPile()
        {
            UnityEngine.Debug.Assert(!DiscardPileEmpty, $"Attempting to peek top card of discard pile when it is empty.");
            return NumCardsInDiscardPile > 0 ? discardPile[NumCardsInDiscardPile - 1] : null;
        }

        #endregion

        #region Removed Pile Management

        public void AddCardToRemovedPile(Card card)
        {
            AddCardToRemovedPile(new CardRuntime(this, card));
        }

        public void AddCardToRemovedPile(CardRuntime cardRuntime)
        {
            removedPile.Add(cardRuntime);
            cardAddedToRemovedPileEvent.Invoke(cardRuntime);
        }

        public CardRuntime RemoveCardFromRemovedPile(int index)
        {
            CardRuntime card = removedPile.Get(index);
            cardRemovedFromRemovedPileEvent.Invoke(card);
            removedPile.RemoveAt(index);

            return card;
        }

        public CardRuntime GetCardInRemovedPile(int index)
        {
            return removedPile.Get(index);
        }

        public CardRuntime PeekTopCardOfRemovedPile()
        {
            UnityEngine.Debug.Assert(!RemovedPileEmpty, $"Attempting to peek top card of removed pile when it is empty.");
            return NumCardsInRemovedPile > 0 ? removedPile[NumCardsInRemovedPile - 1] : null;
        }

        #endregion

        #region Callbacks

        #region Draw Pile Events

        public ICallbackHandle AddCardAddedToDrawPileEventCallback(UnityAction<CardRuntime> callback)
        {
            return cardAddedToDrawPileEvent.AddListener(callback);
        }

        public void RemoveCardAddedToDrawPileEventCallback(ICallbackHandle callback)
        {
            cardAddedToDrawPileEvent.RemoveListener(callback);
        }

        public ICallbackHandle AddCardRemovedFromDrawPileEventCallback(UnityAction<CardRuntime> callback)
        {
            return cardRemovedFromDrawPileEvent.AddListener(callback);
        }

        public void RemoveCardRemovedFromDrawPileEventCallback(ICallbackHandle callback)
        {
            cardRemovedFromDrawPileEvent.RemoveListener(callback);
        }

        #endregion

        #region Discard Pile Events

        public ICallbackHandle AddCardAddedToDiscardPileEventCallback(UnityAction<CardRuntime> callback)
        {
            return cardAddedToDiscardPileEvent.AddListener(callback);
        }

        public void RemoveCardAddedToDiscardPileEventCallback(ICallbackHandle callback)
        {
            cardAddedToDiscardPileEvent.RemoveListener(callback);
        }

        public ICallbackHandle AddCardRemovedFromDiscardPileEventCallback(UnityAction<CardRuntime> callback)
        {
            return cardRemovedFromDiscardPileEvent.AddListener(callback);
        }

        public void RemoveCardRemovedFromDiscardPileEventCallback(ICallbackHandle callback)
        {
            cardRemovedFromDiscardPileEvent.RemoveListener(callback);
        }

        #endregion

        #region Removed Pile Events

        public ICallbackHandle AddCardAddedToRemovedPileEventCallback(UnityAction<CardRuntime> callback)
        {
            return cardAddedToRemovedPileEvent.AddListener(callback);
        }

        public void RemoveCardAddedToRemovedPileEventCallback(ICallbackHandle callback)
        {
            cardAddedToRemovedPileEvent.RemoveListener(callback);
        }

        public ICallbackHandle AddCardRemovedFromRemovedPileEventCallback(UnityAction<CardRuntime> cardRuntime)
        {
            return cardRemovedFromRemovedPileEvent.AddListener(cardRuntime);
        }

        public void RemoveCardRemovedFromRemovedPileEventCallback(ICallbackHandle cardRuntime)
        {
            cardRemovedFromRemovedPileEvent.RemoveListener(cardRuntime);
        }

        #endregion

        #endregion
    }
}