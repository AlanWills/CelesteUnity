using Celeste.Persistence;
using System;
using System.Collections.Generic;

namespace Celeste.DeckBuilding.Persistence
{
    [Serializable]
    public class DeckBuildingDTO : VersionedDTO
    {
        public List<DeckDTO> decks = new List<DeckDTO>();

        public DeckBuildingDTO(DeckBuildingRecord deckRecord)
        {
            decks.Capacity = deckRecord.NumDecks;

            for (int i = 0, n = deckRecord.NumDecks; i < n; ++i)
            {
                decks.Add(new DeckDTO(deckRecord.GetDeck(i)));
            }
        }
    }
}