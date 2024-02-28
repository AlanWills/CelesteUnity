using Celeste.DeckBuilding.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding.Results
{
    [CreateAssetMenu(fileName = nameof(SelectedActorsDefeated), menuName = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM + "Results/Selected Actors Defeated", order = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM_PRIORITY)]
    public class SelectedActorsDefeated : LoseCondition
    {
        #region Properties and Fields

        [SerializeField] private List<Card> cards = new List<Card>();

        private List<bool> cardsDefeated = new List<bool>();

        #endregion

        private void OnValidate()
        {
            for (int i = 0, n = cards.Count; i < n; ++i)
            {
                if (cards[i] != null)
                {
                    UnityEngine.Debug.Assert(cards[i].HasComponent<ActorComponent>(), $"Card {cards[i].name} for {name} has no actor component.");
                }
            }
        }

        protected override void OnHookup()
        {
            cardsDefeated.Capacity = cards.Count;
            cardsDefeated.Clear();

            for (int i = 0, n = cards.Count; i < n; ++i)
            {
                cardsDefeated.Add(false);
            }
        }

        protected override void OnRelease() { }

        public override void OnCardRemovedFromStage(CardRuntime cardRuntime)
        {
            int indexOfCard = cards.FindIndex(x => cardRuntime.IsForCard(x));
            if (indexOfCard >= 0)
            {
                UnityEngine.Debug.Assert(indexOfCard < cardsDefeated.Count, $"Mismatch between cards and cardsDefeated.  Index Of Card {indexOfCard}, CardsDefeated Count {cardsDefeated.Count}.");
                cardsDefeated[indexOfCard] = true;
            }

            for (int i = 0, n = cardsDefeated.Count; i < n; ++i)
            {
                if (!cardsDefeated[i])
                {
                    return;
                }
            }

            OnLose.Invoke();
        }
    }
}