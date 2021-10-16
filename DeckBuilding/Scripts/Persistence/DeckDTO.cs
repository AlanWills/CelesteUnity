using Celeste.DeckBuilding.Decks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding.Persistence
{
    [Serializable]
    public class DeckDTO
    {
        public List<int> cards = new List<int>();

        public DeckDTO(Deck deck)
        {
            cards.Capacity = deck.NumCards;

            for (int i = 0, n = deck.NumCards; i < n; ++i)
            {
                cards.Add(deck.GetCard(i).Guid);
            }
        }
    }
}