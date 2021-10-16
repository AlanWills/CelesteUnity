using Celeste.BT;
using Celeste.DeckBuilding.AI;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Nodes.AI
{
    [CreateNodeMenu("Celeste/Deck Match/Play Increase Max Armour Effect")]
    public class PlayIncreaseMaxArmourEffect : BTNode
    {
        [SerializeField] private UseCardOnActorEvent useCardOnActor;
        [SerializeField] private CardRuntimeEvent useCardOnAll;

        protected override BTNode OnEvaluate(BTBlackboard btBlackboard)
        {
            DeckRuntime controlledDeck = btBlackboard.GetObject<DeckRuntime>(DeckBuildingAIBlackboardKeys.CONTROLLED_DECK);
            CurrentHand currentHand = controlledDeck.CurrentHand;

            for (int i = 0, n = currentHand.NumCards; i < n; ++i)
            {
                CardRuntime card = currentHand.GetCard(i);

                if (card.CanPlay && card.SupportsModifyMaxArmourEffect())
                {
                    // We try and find a target which would benefit from this card
                    // Even if we can target everyone, there's no point using the card
                    // unless at least one actor will benefit from it
                    CardRuntime target = controlledDeck.Stage.FindCard(x => card.CanUseEffectOn(x));

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
    }
}