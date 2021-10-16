using Celeste.Constants;
using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Debug.Menus;
using Celeste.DeckBuilding.Decks;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Extensions;
using Celeste.DeckBuilding.Results;
using Celeste.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding
{
    [AddComponentMenu("Celeste/Deck Building/Deck Runtime")]
    public class DeckRuntime : MonoBehaviour
    {
        #region Properties and Fields

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;

                if (isActive)
                {
                    becameActiveEvent.Invoke();
                }
                else
                {
                    becameInactiveEvent.Invoke();
                }
            }
        }

        public AvailableResources AvailableResources
        {
            get { return availableResources; }
        }

        public CurrentHand CurrentHand
        {
            get { return currentHand; }
        }

        public DrawPile DrawPile
        {
            get { return drawPile; }
        }

        public DiscardPile DiscardPile
        {
            get { return discardPile; }
        }

        public RemovedPile RemovedPile
        {
            get { return removedPile; }
        }

        public Stage Stage
        {
            get { return stage; }
        }

        public Deck Deck
        {
            get { return deck; }
        }

        [Header("Deck Elements")]
        [SerializeField] private AvailableResources availableResources;
        [SerializeField] private CurrentHand currentHand;
        [SerializeField] private DrawPile drawPile;
        [SerializeField] private DiscardPile discardPile;
        [SerializeField] private RemovedPile removedPile;
        [SerializeField] private Stage stage;

        [Header("Data")]
        [SerializeField] private ID id;

        [Header("Events")]
        [SerializeField] private Celeste.Events.Event becameActiveEvent;
        [SerializeField] private Celeste.Events.Event becameInactiveEvent;
        [SerializeField] private CardRuntimeEvent cardPlayedEvent;
        [SerializeField] private Celeste.Events.Event matchLostEvent;

        private Deck deck = default;
        private bool isActive = false;

        #endregion

        public void Hookup(Deck deck)
        {
            this.deck = deck;
            this.deck.LoseCondition.Hookup(Lose);
        }

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref availableResources);
            this.TryGet(ref currentHand);
            this.TryGet(ref drawPile);
            this.TryGet(ref discardPile);
            this.TryGet(ref removedPile);
            this.TryGet(ref stage);
        }

        #endregion

        #region Deck Management

        private void AddCardToDeck(CardRuntime card)
        {
            card.Owner = id;
        }

        public void AddCardToDrawPile(CardRuntime card)
        {
            AddCardToDeck(card);
            drawPile.AddCard(card);
        }

        public void AddCardToHand(CardRuntime card)
        {
            if (!currentHand.IsFull)
            {
                AddCardToDeck(card);
                currentHand.AddCard(card);
                UpdateCanPlayCard(card);
            }
            else
            {
                AddCardToDiscardPile(card);
            }
        }

        public void AddCardToDiscardPile(CardRuntime card)
        {
            AddCardToDeck(card);
            discardPile.AddCard(card);
        }

        public void AddCardToRemovedPile(CardRuntime card)
        {
            AddCardToDeck(card);
            removedPile.AddCard(card);
        }

        public void AddCardToStage(CardRuntime card)
        {
            UnityEngine.Debug.Assert(card.SupportsActor(), $"Could not add card '{card.CardName}' to stage, it does not support it.");
            if (card.SupportsActor())
            {
                AddCardToDeck(card);
                Stage.AddCard(card);
            }
        }

        public void DrawCards(int quantity)
        {
            for (int i = 0; i < quantity; ++i)
            {
                if (drawPile.Empty)
                {
                    for (int discardPileIndex = 0, n = discardPile.NumCards; discardPileIndex < n; ++discardPileIndex)
                    {
                        AddCardToDrawPile(discardPile.GetCard(discardPileIndex));
                    }

                    ShuffleDrawPile();
                    discardPile.Clear();
                }

                if (!drawPile.Empty)
                {
                    AddCardToHand(drawPile.DrawCard());
                }
            }
        }

        public void ShuffleDrawPile()
        {
            drawPile.Shuffle(deck.CardShuffler);
        }

        #endregion

        #region Card Management

        private void UpdateCanPlayCards()
        {
            for (int i = 0, n = CurrentHand.NumCards; i < n; ++i)
            {
                UpdateCanPlayCard(CurrentHand.GetCard(i));
            }
        }

        private void UpdateCanPlayCard(CardRuntime card)
        {
            if (card.HasCardStatus(CardStatus.CannotPlay))
            {
                card.CanPlay = false;
            }
            else if (AvailableResources.CurrentResources < card.GetCost())
            {
                card.CanPlay = false;
            }
            else
            {
                card.CanPlay = true;
            }
        }

        #endregion

        #region Resource Management

        public void AddResources(int quantity)
        {
            AvailableResources.AddResources(quantity);
        }

        public void SetResources(int quantity)
        {
            AvailableResources.SetResources(quantity);
        }

        #endregion

        #region Results

        public void Lose()
        {
            deck.LoseCondition.Release();
            matchLostEvent.Invoke();
        }

        #endregion

        #region Callbacks

        public void OnCardAddedToHand(CardRuntime cardRuntime)
        {
            if (cardRuntime.SupportsCost())
            {
                var costComponent = cardRuntime.FindComponent<CostComponent>();
                costComponent.component.AddOnCostChangedCallback(costComponent.instance, OnCostChanged);
            }

            cardRuntime.OnPlayCardSuccess.AddListener(OnPlayCardSuccess);
        }

        public void OnCardRemovedFromHand(CardRuntime cardRuntime)
        {
            var costComponent = cardRuntime.FindComponent<CostComponent>();
            if (costComponent.IsValid)
            {
                costComponent.component.RemoveOnCostChangedCallback(costComponent.instance, OnCostChanged);
            }
            
            cardRuntime.OnPlayCardSuccess.RemoveListener(OnPlayCardSuccess);
        }

        public void OnResourcesChanged(int newResources)
        {
            UpdateCanPlayCards();
        }

        private void OnCostChanged(CostChangedArgs costChangedArgs)
        {
            UpdateCanPlayCard(costChangedArgs.card);
        }

        private void OnPlayCardSuccess(PlayCardSuccessArgs playArgs)
        {
            CardRuntime card = playArgs.cardRuntime;

            CurrentHand.RemoveCard(card);

            if (card.SupportsCost())
            {
                AddResources(-card.GetCost());
            }

            if (card.IsRemovedFromDeckWhenPlayed())
            {
                AddCardToRemovedPile(card);
            }
            else
            {
                AddCardToDiscardPile(card);
            }

            if (card.SupportsActor())
            {
                AddCardToStage(card);
            }

            cardPlayedEvent.Invoke(card);
        }

        public void OnCardRemovedFromStage(CardRuntime card)
        {
            deck.LoseCondition.OnCardRemovedFromStage(card);
            UpdateCanPlayCards();
        }

        #endregion
    }
}