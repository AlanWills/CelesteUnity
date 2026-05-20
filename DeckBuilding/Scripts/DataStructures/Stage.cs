using Celeste.DataStructures;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding
{
    [CreateAssetMenu(fileName = nameof(Stage), menuName = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM + "Stage", order = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM_PRIORITY)]
    public class Stage : ScriptableObject
    {
        #region Properties and Fields

        public int NumCards
        {
            get { return cards.Count; }
        }

        [SerializeField] private CardRuntimeEvent actorAddedEvent;
        [SerializeField] private CardRuntimeEvent actorRemovedEvent;

        private List<CardInstance> cards = new List<CardInstance>();

        #endregion

        public CardInstance GetCard(int index)
        {
            return cards.Get(index);
        }

        public void AddCard(CardInstance card)
        {
            cards.Add(card);
            card.SetOnStage(true);
            card.AddOnDieCallback(OnCardDied);

            actorAddedEvent.Invoke(card);
        }

        public void RemoveCard(int index)
        {
            CardInstance card = cards.Get(index);

            if (card != null)
            {
                card.SetOnStage(false);
                card.RemoveOnDieCallback(OnCardDied);

                actorRemovedEvent.Invoke(card);
            }
            else
            {
                UnityEngine.Debug.LogAssertion($"Remove Failed: could not find card at index {index} in Stage.");
            }
        }

        public void RemoveCard(CardInstance card)
        {
            if (cards.Remove(card))
            {
                card.SetOnStage(false);
                card.RemoveOnDieCallback(OnCardDied);

                actorRemovedEvent.Invoke(card);
            }
            else
            {
                UnityEngine.Debug.LogAssertion($"Remove Failed: could not find {card.CardName} in Stage.");
            }
        }

        public CardInstance FindCard(Predicate<CardInstance> predicate)
        {
            return cards.Find(predicate);
        }

        public CardInstance FindKillableCard(int damage)
        {
            return FindCard(x => x.SupportsHealth() && x.GetHealth() <= damage);
        }

        #region Callbacks

        private void OnCardDied(DieArgs dieArgs)
        {
            RemoveCard(dieArgs.cardInstance);
        }

        #endregion
    }
}