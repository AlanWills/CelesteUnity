using UnityEngine;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Deck Runtime UI Manager")]
    public class DeckRuntimeUIManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private DeckMatchPlayerUIManager deckController;
        [SerializeField] private StageUIManager stageUIManager;

        #endregion

        public void Hookup(DeckMatchPlayerRuntime playerRuntime)
        {
            deckController.Hookup(
                playerRuntime.AvailableResources,
                playerRuntime.CurrentHand,
                playerRuntime.Deck);
            
            stageUIManager.Hookup(playerRuntime.Stage);
        }
    }
}