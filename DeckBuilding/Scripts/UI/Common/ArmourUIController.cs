using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Extensions;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Armour UI Controller")]
    public class ArmourUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private GameObject armourUI;
        [SerializeField] private TextMeshProUGUI armourText;

        private CardRuntime card;

        #endregion

        public void Hookup(CardRuntime card)
        {
            this.card = card;

            bool supportsArmour = card.SupportsArmour();
            armourUI.SetActive(supportsArmour);

            if (supportsArmour)
            {
                card.AddOnArmourChangedCallback(OnArmourChanged);
                UpdateUI(card.GetArmour());
            }
        }

        private void OnDisable()
        {
            if (card.SupportsArmour())
            {
                card.RemoveOnArmourChangedCallback(OnArmourChanged);
            }
        }

        private void UpdateUI(int armour)
        {
            armourText.text = armour.ToString();
        }

        #region Callbacks

        private void OnArmourChanged(ArmourChangedArgs armourChangedArgs)
        {
            UpdateUI(armourChangedArgs.newArmour);
        }

        #endregion
    }
}