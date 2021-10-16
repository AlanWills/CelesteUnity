using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Extensions;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Health UI Controller")]
    public class HealthUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private GameObject healthUI;
        [SerializeField] private TextMeshProUGUI healthText;

        private CardRuntime card;

        #endregion

        public void Hookup(CardRuntime card)
        {
            this.card = card;

            bool supportsHealth = card.SupportsHealth();
            healthUI.SetActive(supportsHealth);

            if (supportsHealth)
            {
                card.AddOnHealthChangedCallback(OnHealthChanged);
                UpdateUI(card.GetHealth());
            }
        }

        private void OnDisable()
        {
            if (card.SupportsHealth())
            {
                card.RemoveOnHealthChangedCallback(OnHealthChanged);
            }
        }

        private void UpdateUI(int health)
        {
            healthText.text = health.ToString();
        }

        #region Callbacks

        private void OnHealthChanged(HealthChangedArgs healthChangedArgs)
        {
            UpdateUI(healthChangedArgs.newHealth);
        }

        #endregion
    }
}