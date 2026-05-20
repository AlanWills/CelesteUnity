using Celeste.BT;
using Celeste.DeckBuilding.AI;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Nodes.AI
{
    [CreateNodeMenu("Celeste/Deck Match/Play Heal Health Effect")]
    public class PlayHealHealthEffect : BTNode
    {
        [SerializeField] private UseCardOnActorEvent useCardOnActor;
        [SerializeField] private CardRuntimeEvent useCardOnAll;

        protected override BTNode OnEvaluate(BTBlackboard btBlackboard)
        {
            DeckMatchPlayerRuntime controlledDeck = btBlackboard.GetObject<DeckMatchPlayerRuntime>(DeckBuildingAIBlackboardKeys.CONTROLLED_DECK);
            CurrentHand currentHand = controlledDeck.CurrentHand;

            for (int i = 0, n = currentHand.NumCards; i < n; ++i)
            {
                CardInstance card = currentHand.GetCard(i);

                if (card.CanPlay && card.SupportsHealHealthEffect())
                {
                    // We try and find a target which would benefit from this card
                    // Even if we can target everyone, there's no point using the card
                    // unless at least one actor will benefit from it
                    CardInstance target = FindBestTarget(card, controlledDeck.Stage);

                    if (target != null)
                    {
                        if (card.EffectRequiresTarget())
                        {
                            useCardOnActor.Invoke(new UseCardOnActorArgs()
                            {
                                cardInstance = card,
                                actor = target
                            });
                        }
                        else
                        {
                            useCardOnAll.Invoke(card);
                        }

                        return this;
                    }
                }
            }

            return GetDefaultOutputConnectedNode();
        }

        private CardInstance FindBestTarget(CardInstance effect, Stage stage)
        {
            CardInstance bestTarget = null;
            int bestHealthDiff = 0;

            // Find the target which would benefit the most from having its health healed
            for (int i = 0, n = stage.NumCards; i < n; ++i)
            {
                CardInstance actor = stage.GetCard(i);
                
                if (effect.CanUseEffectOn(actor))
                {
                    int currentHealth = actor.GetHealth();
                    int maxHealth = actor.GetMaxHealth();
                    int healthDiff = maxHealth - currentHealth;

                    if (healthDiff > bestHealthDiff)
                    {
                        bestTarget = actor;
                        bestHealthDiff = healthDiff;
                    }
                }
            }

            return bestTarget;
        }
    }
}