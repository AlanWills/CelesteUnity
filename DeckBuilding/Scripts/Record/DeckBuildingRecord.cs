﻿using Celeste.DataStructures;
using Celeste.DeckBuilding.Decks;
using Celeste.DeckBuilding.Shuffler;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding.Persistence
{
    [CreateAssetMenu(fileName = nameof(DeckBuildingRecord), menuName = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM + "Deck Building Record", order = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM_PRIORITY)]
    public class DeckBuildingRecord : ScriptableObject
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