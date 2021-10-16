using Celeste.BT;
using Celeste.DeckBuilding.AI;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Nodes.AI
{
    [CreateNodeMenu("Celeste/Deck Match/Can Play Any Card")]
    public class CanPlayAnyCard : BTNode
    {
        private const string TRUE_OUTPUT_PORT = "True";
        private const string FALSE_OUTPUT_PORT = "False";

        public CanPlayAnyCard()
        {
            RemoveDefaultOutputPort();
            AddOutputPort(TRUE_OUTPUT_PORT);
            AddOutputPort(FALSE_OUTPUT_PORT);
        }

        protected override BTNode OnEvaluate(BTBlackboard btBlackboard)
        {
            DeckRuntime deckRuntime = btBlackboard.GetObject<DeckRuntime>(DeckBuildingAIBlackboardKeys.CONTROLLED_DECK);
            CurrentHand currentHand = deckRuntime.CurrentHand;

            for (int i = 0, n = currentHand.NumCards; i < n; ++i)
            {
                if (!currentHand.GetCard(i).CanPlay)
                {
                    return GetConnectedNode(FALSE_OUTPUT_PORT);
                }
            }

            return GetConnectedNode(TRUE_OUTPUT_PORT);
        }
    }
}