using Celeste.DeckBuilding.Events;
using Celeste.FSM.Nodes.Events;

namespace Celeste.DeckBuilding.Nodes
{
    [CreateNodeMenu("Celeste/Events/Raisers/Draw Cards From Deck Raiser")]
    public class DrawCardsFromDeckRaiserNode : ParameterisedEventRaiserNode<DrawCardsFromDeckArgs, DrawCardsFromDeckEvent>
    {
    }
}
