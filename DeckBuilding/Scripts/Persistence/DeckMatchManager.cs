using Celeste.Components;
using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Catalogue;
using Celeste.DeckBuilding.Decks;
using Celeste.DeckBuilding.Events;
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

        [SerializeField] private CardCatalogue cardCatalogue;

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

        private void Deserialize(DeckMatchPlayerRuntime playerRuntime, DeckMatchPlayerRuntimeDTO playerRuntimeDTO)
        {
            playerRuntime.SetResources(playerRuntimeDTO.availableResources);
            playerRuntime.ClearHand();
            playerRuntime.ClearStage();

            foreach (var cardRuntimeDTO in playerRuntimeDTO.cardsInHand)
            {
                playerRuntime.AddCardToHand(CreateCardRuntime(cardRuntimeDTO, playerRuntime.Deck));
            }

            foreach (var cardRuntimeDTO in playerRuntimeDTO.cardsOnStage)
            {
                playerRuntime.AddCardToStage(CreateCardRuntime(cardRuntimeDTO, playerRuntime.Deck));
            }
        }

        private CardRuntime CreateCardRuntime(CardRuntimeDTO cardRuntimeDTO, Deck deck)
        {
            Card card = cardCatalogue.FindByGuid(cardRuntimeDTO.cardGuid);
            CardRuntime cardRuntime = new CardRuntime(deck, card);
            cardRuntime.LoadComponents(cardRuntimeDTO.components.ToLookup());

            return cardRuntime;
        }

        private void SetDefaultValues(DeckMatchPlayerRuntime deckRuntime)
        {
            deckRuntime.SetResources(0);
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