using Celeste.DataStructures;
using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Results;
using Celeste.DeckBuilding.Shuffler;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding.Decks
{
    public class Deck
    {
        #region Properties and Fields

        public CardShuffler CardShuffler { get; }
        public LoseCondition LoseCondition { get; set; } = ScriptableObject.CreateInstance<SelectedActorsDefeated>();

        public int NumCards
        {
            get { return cards.Count; }
        }

        private List<Card> cards = new List<Card>();

        #endregion

        public Deck() : this(ScriptableObject.CreateInstance<RNGCardShuffler>())
        {
        }

        public Deck(CardShuffler cardShuffler)
        {
            CardShuffler = cardShuffler;
        }

        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        public Card GetCard(int cardIndex)
        {
            return cards.Get(cardIndex);
        }
    }
}