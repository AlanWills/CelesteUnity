using Celeste.DeckBuilding.Match;
using Celeste.Scene.Events;
using System;

namespace Celeste.DeckBuilding.Loading
{
    [Serializable]
    public class DeckMatchContext : Context
    {
        public DeckMatch deckMatch;

        public DeckMatchContext(DeckMatch deckMatch)
        {
            this.deckMatch = deckMatch;
        }
    }
}