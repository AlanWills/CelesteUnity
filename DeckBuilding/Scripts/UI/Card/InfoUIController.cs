using Celeste.DeckBuilding.Cards;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Info UI Controller")]
    public class InfoUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Image cardSprite;
        [SerializeField] private TextMeshProUGUI cardDisplayName;

        #endregion

        public void Hookup(CardRuntime card)
        {
            var infoComponent = card.FindComponent<InfoComponent>();
            if (infoComponent.IsValid)
            {
                cardSprite.sprite = infoComponent.component.GetSprite(infoComponent.instance);
                cardDisplayName.text = infoComponent.component.GetDisplayName(infoComponent.instance);
            }
        }
    }
}