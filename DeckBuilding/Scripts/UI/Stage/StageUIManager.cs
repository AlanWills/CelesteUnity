using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Stage UI Manager")]
    public class StageUIManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private ActorUIManager cardActorUIManager;

        #endregion

        public void Hookup(Stage stage)
        {
            cardActorUIManager.Hookup(stage);
        }
    }
}