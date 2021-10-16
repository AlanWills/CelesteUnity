using Celeste.BT;
using Celeste.DeckBuilding.AI;
using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Nodes.AI
{
    [CreateNodeMenu("Celeste/Deck Match/Play Actor")]
    public class PlayActor : BTNode
    {
        protected override BTNode OnEvaluate(BTBlackboard btBlackboard)
        {
            DeckRuntime controlledDeck = btBlackboard.GetObject<DeckRuntime>(DeckBuildingAIBlackboardKeys.CONTROLLED_DECK);
            CurrentHand currentHand = controlledDeck.CurrentHand;

            for (int i = 0, n = currentHand.NumCards; i < n; ++i)
            {
                CardRuntime card = currentHand.GetCard(i);

                if (card.CanPlay && 
                    card.SupportsActor() &&
                    card.TryPlay())
                {
                    return this;
                }
            }

            return GetDefaultOutputConnectedNode();
        }
    }
}