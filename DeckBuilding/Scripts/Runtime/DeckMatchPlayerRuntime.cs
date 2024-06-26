using Celeste.Constants;
using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Decks;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Extensions;
using UnityEngine;

namespace Celeste.DeckBuilding
{
    [AddComponentMenu("Celeste/Deck Building/Player Owned Deck Runtime")]
    public class DeckMatchPlayerRuntime : MonoBehaviour
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

        public AvailableResources AvailableResources => availableResources;
        public CurrentHand CurrentHand => currentHand;
        public Stage Stage => stage;
        public Deck Deck => deck;

        [Header("Deck Elements")]
        [SerializeField] private AvailableResources availableResources;
        [SerializeField] private Deck deck;
        [SerializeField] private CurrentHand currentHand;
        [SerializeField] private Stage stage;

        [Header("Data")]
        [SerializeField] private ID playerID;

        [Header("Events")]
        [SerializeField] private Celeste.Events.Event becameActiveEvent;
        [SerializeField] private Celeste.Events.Event becameInactiveEvent;
        [SerializeField] private CardRuntimeEvent cardPlayedEvent;

        private bool isActive = false;

        #endregion

        #region Deck Management

        public void ClearHand()
        {
            for (int i = CurrentHand.NumCards - 1; i >= 0; --i)
            {
                CurrentHand.RemoveCard(CurrentHand.GetCard(i));
            }
        }

        public void AddCardToDrawPile(CardRuntime card)
        {
            deck.AddCardToDrawPile(card);
        }

        public void AddCardToHand(CardRuntime card)
        {
            if (!currentHand.IsFull)
            {
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
            deck.AddCardToDiscardPile(card);
        }

        public void AddCardToRemovedPile(CardRuntime card)
        {
            deck.AddCardToRemovedPile(card);
        }

        public void ClearStage()
        {
            for (int i = Stage.NumCards - 1; i >= 0; --i)
            {
                Stage.RemoveCard(i);
            }
        }

        public void AddCardToStage(CardRuntime card)
        {
            UnityEngine.Debug.Assert(card.SupportsActor(), $"Could not add card '{card.CardName}' to stage, it does not support it.");
            if (card.SupportsActor())
            {
                Stage.AddCard(card);
            }
        }

        public void DrawCards(int quantity)
        {
            for (int i = 0; i < quantity; ++i)
            {
                if (!deck.DrawPileEmpty)
                {
                    AddCardToHand(deck.DrawCard());
                }
            }
        }

        public void ShuffleDrawPile()
        {
            deck.ShuffleDrawPile();
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

        private void OnPlayCardSuccess(CardRuntime card)
        {
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
            UpdateCanPlayCards();
        }

        #endregion
    }
}