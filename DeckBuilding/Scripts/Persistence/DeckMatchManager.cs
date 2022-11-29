using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Decks;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Extensions;
using Celeste.DeckBuilding.Persistence;
using Celeste.FSM;
using Celeste.Persistence;
using UnityEngine;

namespace Celeste.DeckBuilding
{
    [AddComponentMenu("Celeste/Deck Building/Deck Match Manager")]
    public class DeckMatchManager : PersistentSceneManager<DeckMatchManager, DeckMatchDTO>
    {
        #region Deck Match Persistence Context

        private struct DeckMatchPersistenceContext
        {
            public DeckMatchRuntime deckMatchRuntime;
            public FSMRuntime deckMatchFSMRuntime;

            public DeckMatchPersistenceContext(LoadDeckMatchArgs loadDeckMatchArgs)
            {
                deckMatchRuntime = loadDeckMatchArgs.deckMatchRuntime;
                deckMatchFSMRuntime = loadDeckMatchArgs.deckMatchFSMRuntime;
            }

            public DeckMatchPersistenceContext(SaveDeckMatchArgs saveDeckMatchArgs)
            {
                deckMatchRuntime = saveDeckMatchArgs.deckMatchRuntime;
                deckMatchFSMRuntime = saveDeckMatchArgs.deckMatchFSMRuntime;
            }
        }

        #endregion

        #region Properties and Fields

        public static readonly string FILE_NAME = "DeckMatch.dat";
        protected override string FileName
        {
            get { return FILE_NAME; }
        }

        private DeckMatchPersistenceContext persistenceContext;

        #endregion

        #region Save/Load Methods

        protected override void Deserialize(DeckMatchDTO dto)
        {
            DeckMatchRuntime deckMatchRuntime = persistenceContext.deckMatchRuntime;
            FSMRuntime deckMatchFSMRuntime = persistenceContext.deckMatchFSMRuntime;
            UnityEngine.Debug.Assert(deckMatchRuntime != null, $"Deck Match Runtime is null when trying to load.");
            UnityEngine.Debug.Assert(deckMatchFSMRuntime != null, $"Deck Match FSM Runtime is null when trying to load.");

            Deserialize(deckMatchRuntime.PlayerDeckRuntime, dto.playerDeckRuntime);
            Deserialize(deckMatchRuntime.EnemyDeckRuntime, dto.enemyDeckRuntime);

            deckMatchRuntime.PlayerDeckRuntime.IsActive = dto.isPlayerTurn;
            deckMatchRuntime.EnemyDeckRuntime.IsActive = !dto.isPlayerTurn;

            if (!string.IsNullOrEmpty(dto.currentFSMRuntimeNode))
            {
                deckMatchFSMRuntime.StartNode = deckMatchFSMRuntime.graph.FindNode(x => string.CompareOrdinal(x.Guid, dto.currentFSMRuntimeNode) == 0);
            }
        }

        protected override DeckMatchDTO Serialize()
        {
            DeckMatchRuntime deckMatchRuntime = persistenceContext.deckMatchRuntime;
            FSMRuntime deckMatchFSMRuntime = persistenceContext.deckMatchFSMRuntime;
            UnityEngine.Debug.Assert(deckMatchRuntime != null, $"Deck Match Runtime is null when trying to save.");
            UnityEngine.Debug.Assert(deckMatchFSMRuntime != null, $"Deck Match FSM Runtime is null when trying to save.");

            return new DeckMatchDTO(deckMatchRuntime, deckMatchFSMRuntime);
        }

        protected override void SetDefaultValues()
        {
            DeckMatchRuntime deckMatchRuntime = persistenceContext.deckMatchRuntime;
            UnityEngine.Debug.Assert(deckMatchRuntime, $"Deck Match Runtime is null when trying to load default state.");

            SetDefaultValues(deckMatchRuntime.PlayerDeckRuntime);
            SetDefaultValues(deckMatchRuntime.EnemyDeckRuntime);

            deckMatchRuntime.PlayerDeckRuntime.IsActive = true;
            deckMatchRuntime.EnemyDeckRuntime.IsActive = false;
        }

        #endregion

        private void Deserialize(DeckMatchPlayerRuntime deckRuntime, DeckMatchPlayerRuntimeDTO deckRuntimeDTO)
        {
            Deck deck = deckRuntime.Deck;

            deckRuntime.SetResources(deckRuntimeDTO.availableResources);

            foreach (var cardRuntimeDTO in deckRuntimeDTO.cardsInDrawPile)
            {
                CardRuntime cardRuntime = CreateCardRuntime(deck, cardRuntimeDTO);
                deckRuntime.AddCardToDrawPile(cardRuntime);
            }

            foreach (var cardRuntimeDTO in deckRuntimeDTO.cardsInHand)
            {
                CardRuntime cardRuntime = CreateCardRuntime(deck, cardRuntimeDTO);
                deckRuntime.AddCardToHand(cardRuntime);
            }

            foreach (var cardRuntimeDTO in deckRuntimeDTO.cardsInDiscardPile)
            {
                CardRuntime cardRuntime = CreateCardRuntime(deck, cardRuntimeDTO);
                deckRuntime.AddCardToDiscardPile(cardRuntime);
            }

            foreach (var cardRuntimeDTO in deckRuntimeDTO.cardsInRemovedPile)
            {
                CardRuntime cardRuntime = CreateCardRuntime(deck, cardRuntimeDTO);
                deckRuntime.AddCardToRemovedPile(cardRuntime);
            }

            foreach (var cardRuntimeDTO in deckRuntimeDTO.cardsOnStage)
            {
                CardRuntime cardRuntime = CreateCardRuntime(deck, cardRuntimeDTO);
                deckRuntime.AddCardToStage(cardRuntime);
            }
        }

        private CardRuntime CreateCardRuntime(Deck deck, CardRuntimeDTO cardRuntimeDTO)
        {
            Card card = deck.GetCardFromDeck(cardRuntimeDTO.deckIndex);
            CardRuntime cardRuntime = new CardRuntime(card, cardRuntimeDTO.deckIndex);
            cardRuntime.LoadComponents(cardRuntimeDTO.componentNames, cardRuntimeDTO.componentData);

            return cardRuntime;
        }

        private void SetDefaultValues(DeckMatchPlayerRuntime deckRuntime)
        {
            var deck = deckRuntime.Deck;

            deckRuntime.SetResources(0);

            for (int i = 0, n = deck.NumCards; i < n; ++i)
            {
                CardRuntime card = new CardRuntime(deck.GetCardFromDeck(i), i);
                if (card.SupportsActor() && card.IsOnStage())
                {
                    deckRuntime.AddCardToStage(card);
                }
                else
                {
                    deckRuntime.AddCardToDrawPile(card);
                }
            }
        }

        #region Callbacks

        public void OnLoadDeckMatch(LoadDeckMatchArgs loadDeckMatchArgs)
        {
            persistenceContext = new DeckMatchPersistenceContext(loadDeckMatchArgs);
            Load();
        }

        public void OnSaveDeckMatch(SaveDeckMatchArgs saveDeckMatchArgs)
        {
            persistenceContext = new DeckMatchPersistenceContext(saveDeckMatchArgs);
            Save();
        }

        #endregion
    }
}