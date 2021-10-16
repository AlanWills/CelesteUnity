using Celeste.DeckBuilding.Decks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Deck Runtime UI Manager")]
    public class DeckRuntimeUIManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private DeckUIManager deckController;
        [SerializeField] private StageUIManager stageUIManager;

        #endregion

        public void Hookup(DeckRuntime deckRuntime)
        {
            deckController.Hookup(
                deckRuntime.AvailableResources,
                deckRuntime.CurrentHand,
                deckRuntime.DrawPile,
                deckRuntime.DiscardPile,
                deckRuntime.RemovedPile);
            
            stageUIManager.Hookup(deckRuntime.Stage);
        }
    }
}