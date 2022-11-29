using Celeste.DeckBuilding.Decks;
using Celeste.Tools;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Deck Match Player UI Manager")]
    public class DeckMatchPlayerUIManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private AvailableResourcesUIController availableResourcesController;
        [SerializeField] private CurrentHandUIController currentHandController;
        [SerializeField] private DeckUIController deckUIController;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref availableResourcesController);
            this.TryGet(ref currentHandController);
            this.TryGet(ref deckUIController);
            this.TryGet(ref discardPileController);
            this.TryGet(ref removedPileController);
        }

        #endregion

        public void Hookup(
            AvailableResources availableResources,
            CurrentHand currentHand,
            Deck deck)
        {
            availableResourcesController.Hookup(availableResources.CurrentResources);
            currentHandController.Hookup(currentHand);
            deckUIController.Hookup(deck);
        }
    }
}