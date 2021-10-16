using Celeste.Tools;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Deck UI Manager")]
    public class DeckUIManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private AvailableResourcesUIController availableResourcesController;
        [SerializeField] private CurrentHandUIController currentHandController;
        [SerializeField] private DrawPileUIController drawPileController;
        [SerializeField] private DiscardPileUIController discardPileController;
        [SerializeField] private RemovedPileUIController removedPileController;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref availableResourcesController);
            this.TryGet(ref currentHandController);
            this.TryGet(ref drawPileController);
            this.TryGet(ref discardPileController);
            this.TryGet(ref removedPileController);
        }

        #endregion

        public void Hookup(
            AvailableResources availableResources,
            CurrentHand currentHand,
            DrawPile drawPile,
            DiscardPile discardPile,
            RemovedPile removedPile)
        {
            availableResourcesController.Hookup(availableResources.CurrentResources);
            currentHandController.Hookup(currentHand);
            drawPileController.Hookup(drawPile);
            discardPileController.Hookup(discardPile);
            removedPileController.Hookup(removedPile);
        }
    }
}