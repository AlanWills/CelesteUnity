using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Extensions;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Damage UI Controller")]
    public class DamageUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private GameObject damageUI;
        [SerializeField] private TextMeshProUGUI damageText;

        private CardRuntime card;

        #endregion

        public void Hookup(CardRuntime card)
        {
            this.card = card;

            bool supportsDamageEffect = card.SupportsDamageEffect();
            damageUI.SetActive(supportsDamageEffect);

            if (supportsDamageEffect)
            {
                var damageComponent = card.FindComponent<DamageEffectComponent>();
                damageComponent.component.AddOnDamageChangedCallback(damageComponent.instance, OnDamageChanged);

                UpdateUI(card.GetDamage());
            }
        }

        private void OnDisable()
        {
            if (card.SupportsDamageEffect())
            {
                var damageComponent = card.FindComponent<DamageEffectComponent>();
                damageComponent.component.RemoveOnDamageChangedCallback(damageComponent.instance, OnDamageChanged);
            }
        }

        private void UpdateUI(int damage)
        {
            damageText.text = damage.ToString();
        }

        #region Callbacks

        private void OnDamageChanged(DamageChangedArgs damageChangedArgs)
        {
            UpdateUI(damageChangedArgs.newDamage);
        }

        #endregion
    }
}