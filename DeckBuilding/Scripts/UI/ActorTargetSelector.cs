using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Extensions;
using Celeste.Input;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Actor Target Selector")]
    public class ActorTargetSelector : MonoBehaviour
    {
        #region Properties and Fields

        private CardRuntime _pendingActor;
        private CardRuntime PendingActor
        {
            get { return _pendingActor; }
            set
            {
                _pendingActor = value;
                targetingLine.enabled = _pendingActor != null;
            }
        }

        [SerializeField] private InputState inputState;
        [SerializeField] private ActorUIManager availableTargetsUI;
        [SerializeField] private RectTransform targetingLineTransform;
        [SerializeField] private Image targetingLine;

        [Header("Events")]
        [SerializeField] private AttackActorWithActorEvent attackActorWithActorEvent;
        [SerializeField] private CardRuntimeEvent attackCancelledEvent;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            PendingActor = null;
        }

        private void Update()
        {
            if (PendingActor != null)
            {
                Vector2 mousePosition = inputState.PointerPosition;
                Vector2 currentPosition = transform.position;
                Vector2 diffToTarget = mousePosition - currentPosition;
                targetingLineTransform.sizeDelta = new Vector2(10, diffToTarget.magnitude / targetingLineTransform.lossyScale.y);
                targetingLineTransform.SetPositionAndRotation(currentPosition + diffToTarget * 0.5f, Quaternion.AngleAxis(Vector2.SignedAngle(Vector3.up, diffToTarget), Vector3.forward));

                if (inputState.LeftMouseButton.wasPressedThisFrame)
                {
                    // Find target first - maybe keep a list of valid targets as they're added to the UI?
                    ActorUIController target = availableTargetsUI.FindCardActorUI(x => x.IsMouseOver);
                    if (target != null)
                    {
                        AttackActorWithPendingActor(target.Card);
                    }
                }
                else if (inputState.RightMouseButton.wasPressedThisFrame)
                {
                    CancelAttack();
                }
            }
        }

        #endregion

        private void AttackActorWithPendingActor(CardRuntime target)
        {
            UnityEngine.Debug.Assert(target != null, "Target for Pending Actor is null.  Cannot attack.");
            attackActorWithActorEvent.Invoke(new AttackActorWithActorArgs() { attacker = PendingActor, target = target });
            PendingActor = null;
        }

        private void CancelAttack()
        {
            UnityEngine.Debug.Assert(PendingActor != null, "Pending Actor is null.  Could not cancel attack.");
            attackCancelledEvent.Invoke(PendingActor);
            PendingActor = null;
        }

        #region Callbacks

        public void OnActorAttackBegun(BeginAttackActorArgs beginAttackActorArgs)
        {
            if (beginAttackActorArgs.attacker.SupportsCombat())
            {
                UnityEngine.Debug.Assert(PendingActor == null, $"Pending Attack already in progress.");
                PendingActor = beginAttackActorArgs.attacker;
                transform.position = beginAttackActorArgs.position;
            }
        }

        #endregion
    }
}