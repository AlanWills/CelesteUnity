using Celeste.Events;
using System.Collections;
using UnityEngine;
using Event = Celeste.Events.Event;

namespace Celeste.UI.Popups
{
    [AddComponentMenu("Celeste/UI/Popup")]
    public class Popup : MonoBehaviour
    {
        #region Properties and Fields

        private static int CLOSED_ANIMATION_NAME = Animator.StringToHash("Closed");

        public GameObject PopupRoot => popupRoot;

        [SerializeField] private PopupRecord popupRecord;
        [SerializeField] private GameObject popupRoot;
        [SerializeField] private Animator animator;

        [Header("Popup Settings")]
        [SerializeField] private bool hideOnConfirm = true;

        [Header("Required Events")]
        [SerializeField] private ShowPopupEvent showPopup;

        [Header("Optional Events")]
        [SerializeField] private Event onHidePopup;
        [SerializeField] private Event onConfirmPressed;
        [SerializeField] private Event onClosePressed;

        private IPopupController popupController;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            Debug.Assert(showPopup != null, $"No 'showPopup' event found on Popup {gameObject.name}.");
            showPopup.AddListener(OnEventRaised);
            popupController = GetComponent<IPopupController>();
            Debug.Assert(popupController != null, $"No popup controller found on popup {gameObject.name}.");
        }

        private void OnDestroy()
        {
            showPopup.RemoveListener(OnEventRaised);
        }

        #endregion

        #region Show/Hide

        public void Show(IPopupArgs args)
        {
            // Don't add to record with this call
            ShowInternal(args, false);
        }

        private void ShowInternal(IPopupArgs args, bool notifyRecord)
        {
            if (notifyRecord)
            {
                popupRecord.OnPopupShown(this, args);
            }

            popupRoot.SetActive(true);
            
            if (popupController != null)
            {
                popupController.OnShow(args);
            }
        }

        public void Hide()
        {
            HideInternal(false);
        }

        private void HideInternal(bool notifyRecord)
        {
            StartCoroutine(HideCoroutine(notifyRecord));
        }

        private IEnumerator HideCoroutine(bool notifyRecord)
        {
            if (animator != null)
            {
                animator.SetTrigger(CLOSED_ANIMATION_NAME);
            }

            // The checking of the animation name is just to avoid this continuing on the first frame we transition from idle to closing
            while (animator != null && (animator.GetBool(CLOSED_ANIMATION_NAME) || animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f))
            {
                yield return null;
            }

            if (popupController != null)
            {
                popupController.OnHide();
            }

            if (onHidePopup != null)
            {
                onHidePopup.Invoke();
            }

            popupRoot.SetActive(false);

            if (notifyRecord)
            {
                popupRecord.OnPopupHidden(this);
            }
        }

        #endregion

        #region Callbacks

        private void OnEventRaised(IPopupArgs args)
        {
            ShowInternal(args, true);
        }

        public void OnConfirmButtonPressed()
        {
            if (popupController != null)
            {
                popupController.OnConfirmPressed();
            }

            if (onConfirmPressed != null)
            {
                onConfirmPressed.Invoke();
            }

            if (hideOnConfirm)
            {
                HideInternal(true);
            }
        }

        public void OnCloseButtonPressed()
        {
            if (popupController != null)
            {
                popupController.OnClosePressed();
            }

            if (onClosePressed != null)
            {
                onClosePressed.Invoke();
            }

            HideInternal(true);
        }

        #endregion
    }
}
