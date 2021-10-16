using Celeste.BT;
using Celeste.DeckBuilding.AI;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Nodes.AI
{
    [CreateNodeMenu("Celeste/Deck Match/Play Decrease Cost Effect")]
    public class PlayDecreaseCostEffect : BTNode
    {
        protected override BTNode OnEvaluate(BTBlackboard btBlackboard)
        {
            DeckRuntime controlledDeck = btBlackboard.GetObject<DeckRuntime>(DeckBuildingAIBlackboardKeys.CONTROLLED_DECK);
            CurrentHand currentHand = controlledDeck.CurrentHand;

            for (int i = 0, n = currentHand.NumCards; i < n; ++i)
            {
                CardRuntime card = currentHand.GetCard(i);

                // Should only be able to use this if we have a card which costs more than zero
                // which isn't the card we're currently trying to play
                if (card.CanPlay &&
                    card.SupportsModifyCostEffect() &&
                    currentHand.HasCard(x => x != card && x.SupportsCost() && x.GetCost() > 0) &&
                    card.TryPlay())
                {
                    return this;
                }
            }

            return GetDefaultOutputConnectedNode();
        }
    }
}