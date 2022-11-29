using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    [AddComponentMenu("Celeste/Deck Building/Commands/Deck Match Command Dispatcher")]
    public class DeckMatchCommandDispatcher : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private DeckMatchRuntime deckMatchRuntime;

        private List<IDeckMatchCommand> pendingCommands = new List<IDeckMatchCommand>();

        #endregion

        #region Unity Methods

        private void LateUpdate()
        {
            int currentCommandIndex = 0;

            while (currentCommandIndex < pendingCommands.Count)
            {
                pendingCommands[currentCommandIndex++].Execute(new DeckMatchCommandArgs(this, deckMatchRuntime));
            }
            pendingCommands.Clear();
        }

        #endregion

        #region Command Methods

        public void AddCommand(IDeckMatchCommand command)
        {
            pendingCommands.Add(command);
        }

        public void AddCommands(int quantity, Func<IDeckMatchCommand> commandFactoryFunc)
        {
            for (int i = 0; i < quantity; ++i)
            {
                AddCommand(commandFactoryFunc());
            }
        }

        #endregion

        private void TryUseCardOnActor(CardRuntime card, CardRuntime target)
        {
            if (card.SupportsEffect() && card.CanUseEffectOn(target))
            {
                AddCommand(card.UseEffectOn(target));
            }
        }

        private void TryUseCardOnAllActors(CardRuntime card, DeckMatchPlayerRuntime friendlyDeck, DeckMatchPlayerRuntime enemyDeck)
        {
            if (card.SupportsEffect() && !card.EffectRequiresTarget())
            {
                TryUseCardOnActorsInStage(card, friendlyDeck.Stage);
                TryUseCardOnActorsInStage(card, enemyDeck.Stage);
            }
        }

        private void TryUseCardOnActorsInStage(CardRuntime card, Stage stage)
        {
            for (int i = 0, n = stage.NumCards; i < n; ++i)
            {
                CardRuntime target = stage.GetCard(i);

                if (card.CanUseEffectOn(target))
                {
                    AddCommand(card.UseEffectOn(stage.GetCard(i)));
                }
            }
        }

        #region Callbacks

        #region Shuffle Draw Pile

        public void OnPlayerShufflesDrawPile()
        {
            AddCommand(new PlayerShufflesDrawPile());
        }

        public void OnEnemyShufflesDrawPile()
        {
            AddCommand(new EnemyShufflesDrawPile());
        }

        #endregion

        #region Draw Cards

        public void OnActiveDeckDrawsCards(int quantity)
        {
            AddCommands(quantity, () => { return new ActiveDeckDrawsCard(); });
        }

        public void OnPlayerDeckDrawsCards(int quantity)
        {
            AddCommands(quantity, () => { return new PlayerDeckDrawsCard(); });
        }

        public void OnEnemyDeckDrawsCards(int quantity)
        {
            AddCommands(quantity, () => { return new EnemyDeckDrawsCard(); });
        }

        #endregion

        #region Add Resources

        public void OnActiveDeckAddsResources(int quantity)
        {
            AddCommand(new ActiveDeckAddsResources(quantity));
        }

        public void OnPlayerDeckAddsResources(int quantity)
        {
            AddCommand(new PlayerDeckAddsResources(quantity));
        }

        public void OnEnemyDeckAddsResources(int quantity)
        {
            AddCommand(new EnemyDeckAddsResources(quantity));
        }

        #endregion

        #region Play Cards

        public void OnPlayerDeckPlaysCard(CardRuntime card)
        {
            TryUseCardOnAllActors(card, deckMatchRuntime.PlayerDeckRuntime, deckMatchRuntime.EnemyDeckRuntime);
        }

        public void OnEnemyDeckPlaysCard(CardRuntime card)
        {
            TryUseCardOnAllActors(card, deckMatchRuntime.EnemyDeckRuntime, deckMatchRuntime.PlayerDeckRuntime);
        }

        public void OnPlayerUseCardOnActor(UseCardOnActorArgs args)
        {
            TryUseCardOnActor(args.cardRuntime, args.actor);
        }

        public void OnPlayerCardCancelled(CardRuntime card)
        {
            AddCommand(new PlayerDeckAddsCard(card));

            if (card.SupportsCost())
            {
                AddCommand(new PlayerDeckAddsResources(card.GetCost()));
            }
        }

        public void OnEnemyUseCardOnActor(UseCardOnActorArgs args)
        {
            TryUseCardOnActor(args.cardRuntime, args.actor);
        }

        #endregion

        #region Attack Actors

        public void OnAttackActorWithActor(AttackActorWithActorArgs attackActorWithActorArgs)
        {
            AddCommand(new AttackActor(attackActorWithActorArgs.attacker, attackActorWithActorArgs.target));
            AddCommand(new ExhaustCard(attackActorWithActorArgs.attacker));
        }

        #endregion

        #region Refresh Cards

        public void OnActiveDeckRefreshAllActorsOnStage()
        {
            AddCommand(new ActiveDeckRefreshAllActorsOnStage());
        }

        #endregion

        #region Exhaust Cards

        public void OnActiveDeckExhaustAllActorsOnStage()
        {
            AddCommand(new ActiveDeckExhaustAllActorsOnStage());
        }

        #endregion

        #endregion
    }
}