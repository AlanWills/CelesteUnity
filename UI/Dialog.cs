using Celeste.Events;
using Celeste.Parameters;
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
    [AddComponentMenu("Celeste/UI/Dialog")]
    public class Dialog : MonoBehaviour, IEventListener
    {
        [Serializable]
        public class ShowDialogParams
        {
            public string title;
            public Sprite image;
            [TextArea]
            public string description;
            public StringValue descriptionValue;
            public string confirmButtonText;
            public string closeButtonText;
            public bool showConfirmButton;
            public bool showCloseButton;
            public List<Event> customDialogEvents = new List<Event>();
        }

        #region Properties and Fields

        private static int CLOSED_ANIMATION_NAME = Animator.StringToHash("Closed");

        [SerializeField]
        private TextMeshProUGUI title;

        [SerializeField]
        private Image image;

        [SerializeField]
        private Text description;

        [SerializeField]
        private Text confirmButtonText;

        [SerializeField]
        private Text closeButtonText;

        [SerializeField]
        private Button confirmButton;

        [SerializeField]
        private Button closeButton;

        [SerializeField]
        private Animator animator;
        
        private List<Event> dialogEvents = new List<Event>();

        #endregion

        #region Show/Hide

        public void Show(ShowDialogParams showDialogParams)
        {
            if (title != null)
            {
                title.text = showDialogParams.title;
            }

            if (image != null)
            {
                image.enabled = showDialogParams.image != null;
                image.sprite = showDialogParams.image;
            }

            if (description != null)
            {
                description.text = showDialogParams.descriptionValue != (object)null ? showDialogParams.descriptionValue.Value : showDialogParams.description;
            }

            if (confirmButton != null)
            {
                confirmButton.gameObject.SetActive(showDialogParams.showConfirmButton);

                if (showDialogParams.showConfirmButton)
                {
                    if (confirmButtonText != null)
                    {
                        confirmButtonText.text = showDialogParams.confirmButtonText;
                    }
                }
            }

            if (closeButton != null)
            {
                closeButton.gameObject.SetActive(showDialogParams.showCloseButton);

                if (showDialogParams.showCloseButton)
                {
                    if (closeButtonText != null)
                    {
                        closeButtonText.text = showDialogParams.closeButtonText;
                    }
                }
            }

            dialogEvents.Clear();
            dialogEvents.AddRange(showDialogParams.customDialogEvents);

            foreach (Event customEvent in dialogEvents)
            {
                customEvent.AddEventListener(this);
            }
        }

        public void OnEventRaised()
        {
            animator.SetTrigger(CLOSED_ANIMATION_NAME);
            StartCoroutine(HideCoroutine());
        }

        private IEnumerator HideCoroutine()
        {
            foreach (Event customEvent in dialogEvents)
            {
                customEvent.RemoveEventListener(this);
            }

            dialogEvents.Clear();

            // The checking of the animation name is just to avoid this continuing on the first frame we transition from idle to closing
            while (animator != null && (animator.GetBool(CLOSED_ANIMATION_NAME) || animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f))
            {
                yield return null;
            }

            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }

        #endregion
    }
}
