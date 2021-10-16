using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Commands;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Extensions;
using Celeste.Maths;
using Celeste.Tools;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Card Target Selector")]
    public class CardTargetSelector : MonoBehaviour
    {
        #region Properties and Fields

        private CardRuntime _pendingCard;
        private CardRuntime PendingCard
        {
            get { return _pendingCard; }
            set
            {
                _pendingCard = value;
                targetingLine.enabled = _pendingCard != null;
            }
        }

        [SerializeField] private ActorUIManager playerActorUI;
        [SerializeField] private ActorUIManager enemyActorUI;
        [SerializeField] private RectTransform targetingLineTransform;
        [SerializeField] private Image targetingLine;

        [Header("Events")]
        [SerializeField] private UseCardOnActorEvent useCardOnActorEvent;
        [SerializeField] private CardRuntimeEvent cardCancelledEvent;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            PendingCard = null;
        }

        private void Update()
        {
            if (PendingCard != null)
            {
                Vector2 mousePosition = Input.mousePosition;
                Vector2 currentPosition = transform.position;
                Vector2 diffToTarget = mousePosition - currentPosition;
                targetingLineTransform.sizeDelta = new Vector2(10, diffToTarget.magnitude / targetingLineTransform.lossyScale.y);
                targetingLineTransform.SetPositionAndRotation(currentPosition + diffToTarget * 0.5f, Quaternion.AngleAxis(Vector2.SignedAngle(Vector3.up, diffToTarget), Vector3.forward));

                ActorUIController target = FindTarget();
                bool usableOnTarget = false;

                if (target != null)
                {
                    usableOnTarget = PendingCard.CanUseEffectOn(target.Card);
                    target.IsValidTarget = usableOnTarget;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (usableOnTarget)
                    {
                        UsePendingCardOnActor(target.Card);
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    CancelPendingCard();
                }
            }
        }

        #endregion

        private void UsePendingCardOnActor(CardRuntime target)
        {
            UnityEngine.Debug.Assert(target != null, "Target for pending card is null.  Cannot use card.");
            useCardOnActorEvent.Invoke(new UseCardOnActorArgs() { cardRuntime = PendingCard, actor = target });
            PendingCard = null;
        }

        private void CancelPendingCard()
        {
            UnityEngine.Debug.Assert(PendingCard != null, "Pending Card is null.  Could not cancel.");
            cardCancelledEvent.Invoke(PendingCard);
            PendingCard = null;
        }

        private ActorUIController FindTarget()
        {
            ActorUIController target = playerActorUI.FindCardActorUI(x => x.IsMouseOver);
            return target != null ? target : enemyActorUI.FindCardActorUI(x => x.IsMouseOver);
        }

        #region Callbacks

        public void OnPlayerCardPlayed(CardRuntime card)
        {
            if (card.SupportsEffect())
            {
                UnityEngine.Debug.Assert(PendingCard == null, $"Card effect already in progress.");
                PendingCard = card.SupportsEffect() && card.EffectRequiresTarget() ? card : null;
            }
        }

        #endregion
    }
}