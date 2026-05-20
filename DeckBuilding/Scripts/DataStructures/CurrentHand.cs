using Celeste.DataStructures;
using Celeste.DeckBuilding.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding
{
    [CreateAssetMenu(fileName = nameof(CurrentHand), menuName = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM + "Current Hand", order = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM_PRIORITY)]
    public class CurrentHand : ScriptableObject
    {
        #region Properties and Fields

        public bool IsFull => NumCards == handLimit;
        public int NumCards => cards.Count;

        [SerializeField] private bool cardsEnterFaceUp = true;
        [SerializeField] private int handLimit = 10;

        [Header("Events")]
        [SerializeField] private CardRuntimeEvent cardAddedEvent;
        [SerializeField] private CardRuntimeEvent cardRemovedEvent;

        [NonSerialized] private List<CardInstance> cards = new List<CardInstance>();

        #endregion

        #region Card Management

        public void AddCard(CardInstance card)
        {
            UnityEngine.Debug.Assert(!IsFull, $"Trying to add card to hand, but it is already full ({NumCards}).");
            if (!IsFull)
            {
                card.IsFaceUp = cardsEnterFaceUp;
                card.OnPlayCardSuccess.AddListener(OnPlayCardSuccess);
                cards.Add(card);
                cardAddedEvent.Invoke(card);
            }
        }

        public CardInstance GetCard(int index)
        {
            return cards.Get(index);
        }

        public bool RemoveCard(CardInstance card)
        {
            if (cards.Remove(card))
            {
                card.OnPlayCardSuccess.RemoveListener(OnPlayCardSuccess);
                cardRemovedEvent.Invoke(card);
                return true;
            }

            UnityEngine.Debug.LogAssertion($"Failed to remove {card.CardName} from current hand.");
            return false;
        }

        public bool RemoveCard(int index)
        {
            CardInstance card = cards.Get(index);

            if (card != null)
            {
                card.OnPlayCardSuccess.RemoveListener(OnPlayCardSuccess);
                cardRemovedEvent.Invoke(card);
                return true;
            }

            UnityEngine.Debug.LogAssertion($"Failed to find a card in hand at index {index} in current hand.");
            return false;
        }

        public bool HasCard(Predicate<CardInstance> predicate)
        {
            return cards.Exists(predicate);
        }

        #endregion

        #region Callbacks

        private void OnPlayCardSuccess(CardInstance cardInstance)
        {
            RemoveCard(cardInstance);
        }

        #endregion
    }
}