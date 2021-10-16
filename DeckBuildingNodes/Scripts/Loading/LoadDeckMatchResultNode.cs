using Celeste.DeckBuilding.Loading;
using Celeste.FSM.Nodes.Loading;
using Celeste.Scene.Events;
using System;

namespace Celeste.DeckBuilding.Nodes.Loading
{
    [Serializable]
    [CreateNodeMenu("Celeste/Deck Building/Loading/Load Deck Match Result")]
    public class LoadDeckMatchResultNode : LoadContextNode
    {
        protected override Context CreateContext()
        {
            return new DeckMatchResultContext();
        }
    }
}
