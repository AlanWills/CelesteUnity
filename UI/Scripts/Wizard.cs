using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Event = Celeste.Events.Event;

namespace Celeste.UI
{
    [AddComponentMenu("Celeste/UI/Wizard")]
    public class Wizard : MonoBehaviour
    {
        [Serializable]
        public struct ShowPagedDialogParams
        {
            public string confirmButtonText;
            public bool showCloseButton;
            public List<PageParams> pages;
        }

        [Serializable]
        public struct PageParams
        {
            public string title;
            public Sprite image;
            [TextArea]
            public string description;
        }

        #region Properties and Fields

        private static readonly int CLOSED_ANIMATION_NAME = Animator.StringToHash("Closed");

        public Event ConfirmButtonClicked
        {
            get { return confirmButtonClicked; }
        }

        public Event CloseButtonClicked
        {
            get { return closeButtonClicked; }
        }

        [SerializeField]
        private TextMeshProUGUI title;

        [SerializeField]
        private Image image;

        [SerializeField]
        private Text description;

        [SerializeField]
        private Text confirmButtonText;

        [SerializeField]
        private Button confirmButton;

        [SerializeField]
        private Button closeButton;

        [SerializeField]
        private Button nextPageButton;

        [SerializeField]
        private Button previousPageButton;

        [SerializeField]
        private Event confirmButtonClicked;

        [SerializeField]
        private Event closeButtonClicked;

        [SerializeField]
        private Animator animator;

        private int currentPageIndex = 0;
        private List<PageParams> pages = new List<PageParams>();

        #endregion

        #region Show/Hide

        public void Show(ShowPagedDialogParams showPagedDialogParams)
        {
            pages.Clear();
            pages.AddRange(showPagedDialogParams.pages);

            if (confirmButtonText != null)
            {
                confirmButtonText.text = showPagedDialogParams.confirmButtonText;
            }

            closeButton.gameObject.SetActive(showPagedDialogParams.showCloseButton);

            SetPage(0);
        }

        public void Confirm()
        {
            StartCoroutine(Hide(confirmButtonClicked));
        }

        public void Close()
        {
            StartCoroutine(Hide(closeButtonClicked));
        }

        private IEnumerator Hide(Event eventToTrigger)
        {
            // The checking of the animation name is just to avoid this continuing on the first frame we transition from idle to closing
            while (animator != null && (animator.GetBool(CLOSED_ANIMATION_NAME) || animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f))
            {
                yield return null;
            }

            eventToTrigger.Invoke();
            GameObject.Destroy(gameObject);
        }

        #endregion

        #region Page Utilities

        private void SetPage(int pageIndex)
        {
            PageParams pageParams = pages[currentPageIndex];
            currentPageIndex = pageIndex;

            title.text = pageParams.title;
            image.enabled = pageParams.image != null;
            image.sprite = pageParams.image;
            description.text = pageParams.description;

            previousPageButton.gameObject.SetActive(pageIndex > 0);
            nextPageButton.gameObject.SetActive(pageIndex < pages.Count - 1);
            confirmButton.gameObject.SetActive(pageIndex == pages.Count - 1);
        }

        public void NextPage()
        {
            SetPage(++currentPageIndex);
        }

        public void PreviousPage()
        {
            SetPage(--currentPageIndex);
        }

        #endregion
    }
}
