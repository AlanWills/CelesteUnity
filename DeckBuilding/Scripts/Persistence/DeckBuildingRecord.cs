using Celeste.DataStructures;
using Celeste.DeckBuilding.Decks;
using Celeste.DeckBuilding.Shuffler;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding.Persistence
{
    public class DeckBuildingRecord
    {
        #region Properties and Fields

        public int NumDecks
        {
            get { return decks.Count; }
        }

        private List<Deck> decks = new List<Deck>();

        #endregion

        public Deck CreateDeck()
        {
            Deck deck = new Deck();
            AddDeck(deck);

            return deck;
        }

        public void AddDeck(Deck deck)
        {
            decks.Add(deck);
        }

        public Deck GetDeck(int index)
        {
            return decks.Get(index);
        }
    }
}