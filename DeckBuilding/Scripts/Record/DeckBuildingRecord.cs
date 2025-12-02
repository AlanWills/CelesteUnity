using Celeste.DataStructures;
using Celeste.DeckBuilding.Decks;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding.Persistence
{
    [CreateAssetMenu(fileName = nameof(DeckBuildingRecord), menuName = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM + "Deck Building Record", order = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM_PRIORITY)]
    public class DeckBuildingRecord : ScriptableObject
    {
        #region Properties and Fields

        public int NumDecks => decks.Count;

        private List<Deck> decks = new List<Deck>();

        #endregion

        public void Initialize()
        {
            Shutdown();
        }

        public void Shutdown()
        {
            decks.Clear();
        }

        public Deck CreateDeck()
        {
            Deck deck = CreateInstance<Deck>();
            deck.name = "RuntimeDeck";
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