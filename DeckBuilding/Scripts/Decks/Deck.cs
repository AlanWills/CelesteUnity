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

        [NonSerialized] private List<CardInstance> drawPile = new List<CardInstance>();
        [NonSerialized] private List<CardInstance> discardPile = new List<CardInstance>();
        [NonSerialized] private List<CardInstance> removedPile = new List<CardInstance>();

        #endregion

        #region Draw Pile Management

        public void AddCardToDrawPile(Card card)
        {
            AddCardToDrawPile(new CardInstance(this, card));
        }

        public void AddCardToDrawPile(CardInstance cardInstance)
        {
            drawPile.Add(cardInstance);
            cardAddedToDrawPileEvent.Invoke(cardInstance);
        }

        public CardInstance RemoveCardFromDrawPile(int index)
        {
            CardInstance card = drawPile.Get(index);
            cardRemovedFromDrawPileEvent.Invoke(card);
            drawPile.RemoveAt(index);

            return card;
        }

        public CardInstance DrawCard()
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
            CardInstance card = drawPile[NumCardsInDrawPile - 1];
            drawPile.RemoveAt(NumCardsInDrawPile - 1);
            cardRemovedFromDrawPileEvent.Invoke(card);

            return card;
        }

        private void RemakeDrawPile()
        {
            for (int i = NumCardsInDiscardPile - 1; i >= 0; --i)
            {
                CardInstance card = RemoveCardFromDiscardPile(i);
                AddCardToDrawPile(card);
            }
        }

        public void ShuffleDrawPile()
        {
            cardShuffler.Shuffle(drawPile);
        }

        public CardInstance GetCardInDrawPile(int index)
        {
            return drawPile.Get(index);
        }

        public CardInstance PeekTopCardOfDrawPile()
        {
            UnityEngine.Debug.Assert(!DrawPileEmpty, $"Attempting to peek top card of draw pile when it is empty.");
            return NumCardsInDrawPile > 0 ? drawPile[NumCardsInDrawPile - 1] : null;
        }

        #endregion

        #region Discard Pile Management

        public void AddCardToDiscardPile(Card card)
        {
            AddCardToDiscardPile(new CardInstance(this, card));
        }

        public void AddCardToDiscardPile(CardInstance cardInstance)
        {
            discardPile.Add(cardInstance);
            cardAddedToDiscardPileEvent.Invoke(cardInstance);
        }

        public CardInstance RemoveCardFromDiscardPile(int index)
        {
            CardInstance card = discardPile.Get(index);
            cardRemovedFromDiscardPileEvent.Invoke(card);
            discardPile.RemoveAt(index);

            return card;
        }

        public CardInstance GetCardInDiscardPile(int index)
        {
            return discardPile.Get(index);
        }

        public CardInstance PeekTopCardOfDiscardPile()
        {
            UnityEngine.Debug.Assert(!DiscardPileEmpty, $"Attempting to peek top card of discard pile when it is empty.");
            return NumCardsInDiscardPile > 0 ? discardPile[NumCardsInDiscardPile - 1] : null;
        }

        #endregion

        #region Removed Pile Management

        public void AddCardToRemovedPile(Card card)
        {
            AddCardToRemovedPile(new CardInstance(this, card));
        }

        public void AddCardToRemovedPile(CardInstance cardInstance)
        {
            removedPile.Add(cardInstance);
            cardAddedToRemovedPileEvent.Invoke(cardInstance);
        }

        public CardInstance RemoveCardFromRemovedPile(int index)
        {
            CardInstance card = removedPile.Get(index);
            cardRemovedFromRemovedPileEvent.Invoke(card);
            removedPile.RemoveAt(index);

            return card;
        }

        public CardInstance GetCardInRemovedPile(int index)
        {
            return removedPile.Get(index);
        }

        public CardInstance PeekTopCardOfRemovedPile()
        {
            UnityEngine.Debug.Assert(!RemovedPileEmpty, $"Attempting to peek top card of removed pile when it is empty.");
            return NumCardsInRemovedPile > 0 ? removedPile[NumCardsInRemovedPile - 1] : null;
        }

        #endregion

        #region Callbacks

        #region Draw Pile Events

        public ICallbackHandle AddCardAddedToDrawPileEventCallback(UnityAction<CardInstance> callback)
        {
            return cardAddedToDrawPileEvent.AddListener(callback);
        }

        public void RemoveCardAddedToDrawPileEventCallback(ICallbackHandle callback)
        {
            cardAddedToDrawPileEvent.RemoveListener(callback);
        }

        public ICallbackHandle AddCardRemovedFromDrawPileEventCallback(UnityAction<CardInstance> callback)
        {
            return cardRemovedFromDrawPileEvent.AddListener(callback);
        }

        public void RemoveCardRemovedFromDrawPileEventCallback(ICallbackHandle callback)
        {
            cardRemovedFromDrawPileEvent.RemoveListener(callback);
        }

        #endregion

        #region Discard Pile Events

        public ICallbackHandle AddCardAddedToDiscardPileEventCallback(UnityAction<CardInstance> callback)
        {
            return cardAddedToDiscardPileEvent.AddListener(callback);
        }

        public void RemoveCardAddedToDiscardPileEventCallback(ICallbackHandle callback)
        {
            cardAddedToDiscardPileEvent.RemoveListener(callback);
        }

        public ICallbackHandle AddCardRemovedFromDiscardPileEventCallback(UnityAction<CardInstance> callback)
        {
            return cardRemovedFromDiscardPileEvent.AddListener(callback);
        }

        public void RemoveCardRemovedFromDiscardPileEventCallback(ICallbackHandle callback)
        {
            cardRemovedFromDiscardPileEvent.RemoveListener(callback);
        }

        #endregion

        #region Removed Pile Events

        public ICallbackHandle AddCardAddedToRemovedPileEventCallback(UnityAction<CardInstance> callback)
        {
            return cardAddedToRemovedPileEvent.AddListener(callback);
        }

        public void RemoveCardAddedToRemovedPileEventCallback(ICallbackHandle callback)
        {
            cardAddedToRemovedPileEvent.RemoveListener(callback);
        }

        public ICallbackHandle AddCardRemovedFromRemovedPileEventCallback(UnityAction<CardInstance> cardRuntime)
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