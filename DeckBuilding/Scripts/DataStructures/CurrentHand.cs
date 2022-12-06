using Celeste.DataStructures;
using Celeste.DeckBuilding.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding
{
    [CreateAssetMenu(fileName = nameof(CurrentHand), menuName = "Celeste/Deck Building/Current Hand")]
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

        [NonSerialized] private List<CardRuntime> cards = new List<CardRuntime>();

        #endregion

        #region Card Management

        public void AddCard(CardRuntime card)
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

        public CardRuntime GetCard(int index)
        {
            return cards.Get(index);
        }

        public bool RemoveCard(CardRuntime card)
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

        public bool HasCard(Predicate<CardRuntime> predicate)
        {
            return cards.Exists(predicate);
        }

        #endregion

        #region Callbacks

        private void OnPlayCardSuccess(CardRuntime cardRuntime)
        {
            RemoveCard(cardRuntime);
        }

        #endregion
    }
}