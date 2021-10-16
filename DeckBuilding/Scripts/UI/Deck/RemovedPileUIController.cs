using Celeste.DeckBuilding.Cards;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Removed Pile UI Controller")]
    public class RemovedPileUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private TextMeshProUGUI removedPileCount;

        private RemovedPile removedPile;

        #endregion

        public void Hookup(RemovedPile removedPile)
        {
            this.removedPile = removedPile;

            UpdateUI();
        }

        private void UpdateUI()
        {
            removedPileCount.text = removedPile.NumCards.ToString();
        }

        #region Callbacks

        public void OnCardAdded(CardRuntime card)
        {
            UpdateUI();
        }

        public void OnCleared()
        {
            UpdateUI();
        }

        #endregion
    }
}