

using Celeste.DeckBuilding.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding.Results
{
    [CreateAssetMenu(fileName = nameof(NumberOfActorsDefeated), menuName = "Celeste/Deck Building/Results/Number Of Actors Defeated")]
    public class NumberOfActorsDefeated : LoseCondition
    {
        #region Properties and Fields

        [SerializeField] private List<Card> cardsToDefeat = new List<Card>();
        [SerializeField] private int numberToDefeat = 0;

        private int cardsDefeated = 0;

        #endregion

        private void OnValidate()
        {
            for (int i = 0, n = cardsToDefeat.Count; i < n; ++i)
            {
                if (cardsToDefeat[i] != null)
                {
                    UnityEngine.Debug.Assert(cardsToDefeat[i].HasComponent<ActorComponent>(), $"Card {cardsToDefeat[i].name} for {name} has no actor component.");
                }
            }
        }

        protected override void OnHookup()
        {
            cardsDefeated = 0;
        }

        protected override void OnRelease() { }

        public override void OnCardRemovedFromStage(CardRuntime cardRuntime)
        {
            int indexOfCard = cardsToDefeat.FindIndex(x => cardRuntime.IsForCard(x));
            if (indexOfCard >= 0)
            {
                ++cardsDefeated;
            }

            if (cardsDefeated >= numberToDefeat)
            {
                OnLose.Invoke();
            }
        }
    }
}