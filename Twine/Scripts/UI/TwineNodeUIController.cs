using Celeste.Events;
using Celeste.Memory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Celeste.Twine.UI
{
    [AddComponentMenu("Celeste/Twine/UI/Twine Node UI Controller")]
    public class TwineNodeUIController : MonoBehaviour, IPointerClickHandler
    {
        #region Properties and Fields

        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI bodyText;

        [Header("Events")]
        [SerializeField] private ShowPopupEvent showEditTwineNodePopup;

        private TwineNode twineNode;

        #endregion

        #region Factory Methods

        public static TwineNodeUIController From(TwineNode twineNode, GameObjectAllocator allocator)
        {
            GameObject twineNodeUIGameObject = allocator.AllocateWithResizeIfNecessary();
            if (twineNodeUIGameObject == null)
            {
                UnityEngine.Debug.LogAssertion($"Could not allocate {nameof(TwineNodeUIController)}.");
                return null;
            }

            TwineNodeUIController twineNodeUIController = twineNodeUIGameObject.GetComponent<TwineNodeUIController>();
            if (twineNodeUIController == null)
            {
                allocator.Deallocate(twineNodeUIGameObject);
                UnityEngine.Debug.LogAssertion($"Could not find {nameof(TwineNodeUIController)} on allocated GameObject.");
                return null;
            }

            twineNodeUIController.Hookup(twineNode);
            twineNodeUIGameObject.SetActive(true);
            
            return twineNodeUIController;
        }

        private void Hookup(TwineNode twineNode)
        {
            this.twineNode = twineNode;
            this.twineNode.OnChanged.AddListener(OnTwineNodeChanged);

            transform.position = twineNode.Position;
            
            RefreshUI();
        }

        private void RefreshUI()
        {
            titleText.text = twineNode.name;
            bodyText.text = twineNode.text;
        }

        #endregion

        #region Unity Methods

        private void OnDisable()
        {
            if (twineNode != null)
            {
                twineNode.OnChanged.RemoveListener(OnTwineNodeChanged);
                twineNode = null;
            }
        }

        #endregion

        #region IPointerClickHandler

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount == 2)
            {
                // We've been double clicked, so we show the full popup for our node for editing
                EditTwineNodePopupController.EditTwineNodePopupArgs args = new EditTwineNodePopupController.EditTwineNodePopupArgs()
                {
                    twineNode = twineNode
                };
                showEditTwineNodePopup.Invoke(args);
            }
        }

        #endregion

        #region Callbacks

        private void OnTwineNodeChanged()
        {
            RefreshUI();
        }

        #endregion
    }
}