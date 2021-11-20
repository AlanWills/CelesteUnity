using Celeste.Events;
using System.Collections;
using UnityEngine;

using Event = Celeste.Events.Event;

namespace Celeste.UI
{
    [AddComponentMenu("Celeste/UI/Popup")]
    public class Popup : MonoBehaviour, IEventListener<ShowPopupArgs>
    {
        #region Properties and Fields

        private static int CLOSED_ANIMATION_NAME = Animator.StringToHash("Closed");

        [SerializeField] private GameObject popupRoot;
        [SerializeField] private Animator animator;

        [Header("Required Events")]
        [SerializeField] private ShowPopupEvent showPopup;

        [Header("Optional Events")]
        [SerializeField] private Event onPopupHide;

        private IPopupController popupController;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            Debug.Assert(showPopup != null, $"No 'showPopup' event found on Popup {gameObject.name}.");
            showPopup.AddListener(OnEventRaised);
            popupController = GetComponent<IPopupController>();
        }

        private void OnDestroy()
        {
            showPopup.RemoveListener(OnEventRaised);
        }

        #endregion

        #region Show/Hide

        public void Show(ShowPopupArgs args)
        {
            if (popupController != null)
            {
                popupController.OnShow(args);
            }

            popupRoot.SetActive(true);
        }

        public void Hide()
        {
            if (animator != null)
            {
                animator.SetTrigger(CLOSED_ANIMATION_NAME);
            }

            StartCoroutine(HideCoroutine());
        }

        private IEnumerator HideCoroutine()
        {
            // The checking of the animation name is just to avoid this continuing on the first frame we transition from idle to closing
            while (animator != null && (animator.GetBool(CLOSED_ANIMATION_NAME) || animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f))
            {
                yield return null;
            }

            if (popupController != null)
            {
                popupController.OnHide();
            }

            if (onPopupHide != null)
            {
                onPopupHide.Invoke();
            }

            popupRoot.SetActive(false);
        }

        #endregion

        #region Callbacks

        public void OnEventRaised(ShowPopupArgs args)
        {
            Show(args);
        }

        public void OnConfirmButtonPressed()
        {
            if (popupController != null)
            {
                popupController.OnConfirmPressed();
            }

            Hide();
        }

        public void OnCloseButtonPressed()
        {
            if (popupController != null)
            {
                popupController.OnClosePressed();
            }

            Hide();
        }

        #endregion
    }
}
