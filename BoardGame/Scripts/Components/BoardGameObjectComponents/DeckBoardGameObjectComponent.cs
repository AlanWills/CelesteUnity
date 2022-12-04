using Celeste.Components;
using Celeste.DeckBuilding;
using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Decks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

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
                deck.AddCardToDrawPile(new CardRuntime(card));
                saveData.cardsInDrawPile.Add(card.Guid);
            }

            foreach (Card card in startingDiscardPileCards)
            {
                deck.AddCardToDrawPile(new CardRuntime(card));
                saveData.cardsInDiscardPile.Add(card.Guid);
            }

            foreach (Card card in startingRemovedPileCards)
            {
                deck.AddCardToRemovedPile(new CardRuntime(card));
                saveData.cardsInRemovedPile.Add(card.Guid);
            }
        }
    }
}
