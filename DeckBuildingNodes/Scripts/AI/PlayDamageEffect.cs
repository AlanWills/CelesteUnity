using Celeste.BT;
using Celeste.DataStructures;
using Celeste.DeckBuilding.AI;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding.Nodes.AI
{
    [CreateNodeMenu("Celeste/Deck Match/Play Damage Effect")]
    public class PlayDamageEffect : BTNode
    {
        [SerializeField] private UseCardOnActorEvent useCardOnActor;
        [SerializeField] private CardRuntimeEvent useCardOnAll;

        private List<CardRuntime> cardsCache = new List<CardRuntime>();

        protected override BTNode OnEvaluate(BTBlackboard btBlackboard)
        {
            DeckMatchPlayerRuntime controlledDeck = btBlackboard.GetObject<DeckMatchPlayerRuntime>(DeckBuildingAIBlackboardKeys.CONTROLLED_DECK);
            DeckMatchPlayerRuntime opponentDeck = btBlackboard.GetObject<DeckMatchPlayerRuntime>(DeckBuildingAIBlackboardKeys.OPPONENT_DECK);
            CurrentHand currentHand = controlledDeck.CurrentHand;

            for (int i = 0, n = currentHand.NumCards; i < n; ++i)
            {
                CardRuntime card = currentHand.GetCard(i);

                if (card.CanPlay && card.SupportsDamageEffect())
                {
                    CardRuntime target = FindBestTarget(card, opponentDeck.Stage);

                    // Only use this card if there is actually a valid target
                    if (target != null)
                    {
                        if (card.EffectRequiresTarget())
                        {
                            useCardOnActor.Invoke(new UseCardOnActorArgs()
                            {
                                cardRuntime = card,
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

        private CardRuntime FindBestTarget(CardRuntime effect, Stage opponentStage)
        {
            using (var cache = new ClearScope(cardsCache))
            {
                int damage = effect.GetDamage();
                for (int i = 0, n = opponentStage.NumCards; i < n; ++i)
                {
                    CardRuntime target = opponentStage.GetCard(i);

                    if (effect.CanUseEffectOn(target))
                    {
                        // If we have a killable target we choose it here - otherwise, keep track of
                        // all the targets we can use this effect on and choose one at random
                        int health = target.GetHealth();

                        if (health <= damage)
                        {
                            // Killable target
                            return target;
                        }
                        else
                        {
                            cardsCache.Add(target);
                        }
                    }
                }

                return cardsCache.Count > 0 ? cardsCache[Random.Range(0, cardsCache.Count)] : null;
            }
        }
    }
}