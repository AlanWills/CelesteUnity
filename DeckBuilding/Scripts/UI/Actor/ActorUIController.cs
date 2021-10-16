using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Actor UI Controller")]
    public class ActorUIController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region Properties and Fields

        public CardRuntime Card { get; private set; }

        private bool isMouseOver = false;
        public bool IsMouseOver
        {
            get { return isMouseOver; }
            private set
            {
                isMouseOver = value;
                readyUIController.MouseOver = value;
            }
        }

        public bool IsValidTarget
        {
            set { readyUIController.ValidTarget = value; }
        }

        [SerializeField] private bool interactable = true;

        [Header("UI")]
        [SerializeField] private UnityEngine.UI.Image actorImage;
        [SerializeField] private StrengthUIController strengthUIController;
        [SerializeField] private ArmourUIController armourUIController;
        [SerializeField] private HealthUIController healthUIController;
        [SerializeField] private ReadyUIController readyUIController;

        [Header("Events")]
        [SerializeField] private BeginAttackActorEvent beginAttackActorEvent;

        #endregion

        public void Hookup(CardRuntime card)
        {
            Card = card;
            IsMouseOver = false;

            var actorComponent = card.FindComponent<ActorComponent>();
            UnityEngine.Debug.Assert(actorComponent.IsValid, $"Card {card.CardName} has no {nameof(ActorComponent)}.");
            actorImage.sprite = actorComponent.IsValid ? actorComponent.component.GetSprite(actorComponent.instance) : null;

            strengthUIController.Hookup(card);
            armourUIController.Hookup(card);
            healthUIController.Hookup(card);
            readyUIController.Hookup(card);
        }

        #region IPointer

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsMouseOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsMouseOver = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (interactable && Card.SupportsCombat())
            {
                var combatComponent = Card.FindComponent<CombatComponent>();
                if (combatComponent.component.IsReady(combatComponent.instance))
                {
                    beginAttackActorEvent.Invoke(new BeginAttackActorArgs() { attacker = Card, position = transform.position });
                }
            }
        }

        #endregion
    }
}