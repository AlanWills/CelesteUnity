using Celeste.Components;
using Celeste.DeckBuilding;
using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Catalogue;
using Celeste.DeckBuilding.Decks;
using Celeste.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.BoardGame.Components
{
    [DisplayName("Deck")]
    [CreateAssetMenu(fileName = nameof(DeckBoardGameObjectComponent), menuName = "Celeste/Board Game/Board Game Object Components/Deck")]
    public class DeckBoardGameObjectComponent : BoardGameObjectComponent
    {
        #region Save Data

        [Serializable]
        private class SaveData : ComponentData
        {
            public List<int> cardsInDrawPile = new List<int>();
            public List<int> cardsInDiscardPile = new List<int>();
            public List<int> cardsInRemovedPile = new List<int>();
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private Deck deck;
        [SerializeField] private CardCatalogue cardCatalogue;

        [Header("Default Values")]
        [SerializeField] private List<Card> startingDrawPileCards = new List<Card>();
        [SerializeField] private List<Card> startingDiscardPileCards = new List<Card>();
        [SerializeField] private List<Card> startingRemovedPileCards = new List<Card>();

        [NonSerialized] private ICallbackHandle cardAddedToDrawPileCallback;
        [NonSerialized] private ICallbackHandle cardRemovedFromDrawPileCallback;
        [NonSerialized] private ICallbackHandle cardAddedToDiscardPileCallback;
        [NonSerialized] private ICallbackHandle cardRemovedFromDiscardPileCallback;
        [NonSerialized] private ICallbackHandle cardAddedToRemovedPileCallback;
        [NonSerialized] private ICallbackHandle cardRemovedFromRemovedPileCallback;

        #endregion

        public override ComponentData CreateData()
        {
            return new SaveData();
        }

        public override void SetDefaultValues(Instance instance)
        {
            SaveData saveData = instance.data as SaveData;

            foreach (Card card in startingDrawPileCards)
            {
                deck.AddCardToDrawPile(card);
                saveData.cardsInDrawPile.Add(card.Guid);
            }

            foreach (Card card in startingDiscardPileCards)
            {
                deck.AddCardToDrawPile(card);
                saveData.cardsInDiscardPile.Add(card.Guid);
            }

            foreach (Card card in startingRemovedPileCards)
            {
                deck.AddCardToRemovedPile(card);
                saveData.cardsInRemovedPile.Add(card.Guid);
            }

            AddListeners(instance);
        }

        public override void Load(Instance instance)
        {
            SaveData saveData = instance.data as SaveData;

            foreach (int cardGuid in saveData.cardsInDrawPile)
            {
                Card card = cardCatalogue.FindByGuid(cardGuid);
                Debug.Assert(card != null, $"Could not find card with guid {cardGuid}.");
                deck.AddCardToDrawPile(card);
            }

            foreach (int cardGuid in saveData.cardsInDiscardPile)
            {
                Card card = cardCatalogue.FindByGuid(cardGuid);
                Debug.Assert(card != null, $"Could not find card with guid {cardGuid}.");
                deck.AddCardToDiscardPile(card);
            }

            foreach (int cardGuid in saveData.cardsInRemovedPile)
            {
                Card card = cardCatalogue.FindByGuid(cardGuid);
                Debug.Assert(card != null, $"Could not find card with guid {cardGuid}.");
                deck.AddCardToRemovedPile(card);
            }

            AddListeners(instance);
        }

        public override void Shutdown(Instance instance)
        {
            RemoveListeners();
        }

        #region Callbacks

        private void AddListeners(Instance instance)
        {

            cardAddedToDrawPileCallback = deck.AddCardAddedToDrawPileEventCallback(
                (CardRuntime cardRuntime) =>
                {
                    SaveData saveData = instance.data as SaveData;
                    saveData.cardsInDrawPile.Add(cardRuntime.CardGuid);

                    OnCardInDeckChanged(instance);
                });

            cardRemovedFromDrawPileCallback = deck.AddCardRemovedFromDrawPileEventCallback(
                (CardRuntime cardRuntime) =>
                {
                    SaveData saveData = instance.data as SaveData;
                    saveData.cardsInDrawPile.Remove(cardRuntime.CardGuid);

                    OnCardInDeckChanged(instance);
                });

            cardAddedToDiscardPileCallback = deck.AddCardAddedToDiscardPileEventCallback(
                (CardRuntime cardRuntime) =>
                {
                    SaveData saveData = instance.data as SaveData;
                    saveData.cardsInDiscardPile.Add(cardRuntime.CardGuid);

                    OnCardInDeckChanged(instance);
                });

            cardRemovedFromDiscardPileCallback = deck.AddCardRemovedFromDiscardPileEventCallback(
                (CardRuntime cardRuntime) =>
                {
                    SaveData saveData = instance.data as SaveData;
                    saveData.cardsInDiscardPile.Remove(cardRuntime.CardGuid);

                    OnCardInDeckChanged(instance);
                });

            cardAddedToRemovedPileCallback = deck.AddCardAddedToRemovedPileEventCallback(
                (CardRuntime cardRuntime) =>
                {
                    SaveData saveData = instance.data as SaveData;
                    saveData.cardsInRemovedPile.Add(cardRuntime.CardGuid);

                    OnCardInDeckChanged(instance);
                });

            cardRemovedFromRemovedPileCallback = deck.AddCardRemovedFromRemovedPileEventCallback(
                (CardRuntime cardRuntime) =>
                {
                    SaveData saveData = instance.data as SaveData;
                    saveData.cardsInRemovedPile.Remove(cardRuntime.CardGuid);

                    OnCardInDeckChanged(instance);
                });
        }

        private void RemoveListeners()
        {
            deck.RemoveCardAddedToDrawPileEventCallback(cardAddedToDrawPileCallback);
            deck.RemoveCardRemovedFromDrawPileEventCallback(cardRemovedFromDrawPileCallback);
            deck.RemoveCardAddedToDiscardPileEventCallback(cardAddedToDiscardPileCallback);
            deck.RemoveCardRemovedFromDiscardPileEventCallback(cardRemovedFromDiscardPileCallback);
            deck.RemoveCardAddedToRemovedPileEventCallback(cardAddedToRemovedPileCallback);
            deck.RemoveCardRemovedFromRemovedPileEventCallback(cardRemovedFromRemovedPileCallback);

            cardAddedToDrawPileCallback.MakeInvalid();
            cardRemovedFromDrawPileCallback.MakeInvalid();
            cardAddedToDiscardPileCallback.MakeInvalid();
            cardRemovedFromDiscardPileCallback.MakeInvalid();
            cardAddedToRemovedPileCallback.MakeInvalid();
            cardRemovedFromRemovedPileCallback.MakeInvalid();
        }

        private void OnCardInDeckChanged(Instance instance)
        {
            instance.events.ComponentDataChanged.Invoke();
        }

        #endregion
    }
}
