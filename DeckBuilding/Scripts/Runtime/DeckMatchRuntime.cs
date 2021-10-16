using Celeste.DeckBuilding.Decks;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Loading;
using Celeste.DeckBuilding.Match;
using Celeste.DeckBuilding.Persistence;
using Celeste.DeckBuilding.UI;
using Celeste.FSM;
using Celeste.Scene.Events;
using UnityEngine;

namespace Celeste.DeckBuilding
{
    [AddComponentMenu("Celeste/Deck Building/Deck Match Runtime")]
    public class DeckMatchRuntime : MonoBehaviour
    {
        #region Properties and Fields

        public DeckRuntime ActiveDeckRuntime
        {
            get { return playerDeckRuntime.IsActive ? playerDeckRuntime : enemyDeckRuntime; }
        }

        public DeckRuntime InactiveDeckRuntime
        {
            get { return playerDeckRuntime.IsActive ? enemyDeckRuntime : playerDeckRuntime; }
        }

        public DeckRuntime PlayerDeckRuntime
        {
            get { return playerDeckRuntime; }
        }

        public DeckRuntime EnemyDeckRuntime
        {
            get { return enemyDeckRuntime; }
        }

        [SerializeField] private DeckRuntimeUIManager playerDeckUIManager;
        [SerializeField] private DeckRuntimeUIManager enemyDeckUIManager;
        [SerializeField] private FSMRuntime deckMatchFSMRuntime;

        [Header("Deck Runtimes")]
        [SerializeField] private DeckRuntime playerDeckRuntime;
        [SerializeField] private DeckRuntime enemyDeckRuntime;

        [Header("Events")]
        [SerializeField] private LoadDeckMatchEvent loadDeckMatchEvent;
        [SerializeField] private SaveDeckMatchEvent saveDeckMatchEvent;

        #endregion

        private void Hookup(DeckMatch deckMatch)
        {
            HookupDeck(deckMatch.CreatePlayerDeck(), playerDeckRuntime, playerDeckUIManager);
            HookupDeck(deckMatch.CreateEnemyDeck(), enemyDeckRuntime, enemyDeckUIManager);

            loadDeckMatchEvent.Invoke(new LoadDeckMatchArgs(this, deckMatchFSMRuntime));
            deckMatchFSMRuntime.enabled = true;
        }

        private void HookupDeck(
            Deck deck, 
            DeckRuntime deckRuntime,
            DeckRuntimeUIManager deckRuntimeUI)
        {
            deckRuntimeUI.Hookup(deckRuntime);
            deckRuntime.Hookup(deck);
        }

        #region Callbacks

        public void OnDeckMatchContextLoaded(OnContextLoadedArgs onContextLoadedArgs)
        {
            DeckMatchContext deckMatchContext = onContextLoadedArgs.context as DeckMatchContext;
            Hookup(deckMatchContext.deckMatch);
        }

        public void OnMatchBegin()
        {
            saveDeckMatchEvent.Invoke(new SaveDeckMatchArgs(this, deckMatchFSMRuntime));
        }

        public void OnMatchEnd()
        {
            saveDeckMatchEvent.Invoke(new SaveDeckMatchArgs(this, deckMatchFSMRuntime));
        }

        public void OnBeginTurn() 
        {
            saveDeckMatchEvent.Invoke(new SaveDeckMatchArgs(this, deckMatchFSMRuntime));
        }

        public void OnEndTurn()
        {
            playerDeckRuntime.IsActive = !playerDeckRuntime.IsActive;
            enemyDeckRuntime.IsActive = !enemyDeckRuntime.IsActive;
        }

        #endregion
    }
}