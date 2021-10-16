using Celeste.DeckBuilding.Cards;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Draw UI Pile Controller")]
    public class DrawPileUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private TextMeshProUGUI drawPileCount;

        private DrawPile drawPile;

        #endregion

        public void Hookup(DrawPile drawPile)
        {
            this.drawPile = drawPile;

            UpdateUI();
        }

        private void UpdateUI()
        {
            drawPileCount.text = drawPile.NumCards.ToString();
        }

        #region Callbacks

        public void OnCardAdded(CardRuntime card)
        {
            UpdateUI();
        }

        public void OnCardRemoved(CardRuntime card)
        {
            UpdateUI();
        }

        #endregion
    }
}