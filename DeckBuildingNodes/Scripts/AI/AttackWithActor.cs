using Celeste.BT;
using Celeste.DeckBuilding.AI;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Nodes.AI
{
    [CreateNodeMenu("Celeste/Deck Match/Attack With Actor")]
    public class AttackWithActor : BTNode
    {
        [SerializeField] private AttackActorWithActorEvent attackActorWithActor;

        protected override BTNode OnEvaluate(BTBlackboard btBlackboard)
        {
            DeckRuntime controlledDeck = btBlackboard.GetObject<DeckRuntime>(DeckBuildingAIBlackboardKeys.CONTROLLED_DECK);
            DeckRuntime opponentDeck = btBlackboard.GetObject<DeckRuntime>(DeckBuildingAIBlackboardKeys.OPPONENT_DECK);

            CardRuntime readyActor = controlledDeck.Stage.FindCard(x => x.SupportsCombat() && x.IsReady());
            if (readyActor == null)
            {
                return GetDefaultOutputConnectedNode();
            }

            if (opponentDeck.Stage.NumCards == 0)
            {
                return GetDefaultOutputConnectedNode();
            }

            attackActorWithActor.Invoke(new AttackActorWithActorArgs()
            {
                attacker = readyActor,
                target = FindBestTarget(readyActor, opponentDeck.Stage)
            });
            return this;
        }

        private CardRuntime FindBestTarget(CardRuntime attacker, Stage opponentStage)
        {
            int strength = attacker.GetStrength();
            var killableTarget = opponentStage.FindKillableCard(strength);

            if (killableTarget != null)
            {
                // We've found a target we could kill if we attacked it
                return killableTarget;
            }

            // Otherwise, we choose a random enemy
            return opponentStage.GetCard(Random.Range(0, opponentStage.NumCards));
        }
    }
}