using Celeste.DeckBuilding.Cards;
using Celeste.Memory;
using Celeste.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Available Resources UI Controller")]
    public class AvailableResourcesUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private TextMeshProUGUI availableResourcesText;

        #endregion

        public void Hookup(int availableResources)
        {
            UpdateUI(availableResources);
        }

        private void UpdateUI(int availableResources)
        {
            availableResourcesText.text = $"Resources: {availableResources}";
        }

        #region Callbacks

        public void OnResourcesChanged(int resources)
        {
            UpdateUI(resources);
        }

        #endregion
    }
}