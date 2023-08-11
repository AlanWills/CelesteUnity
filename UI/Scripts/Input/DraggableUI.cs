using Celeste.Maths;
using Celeste.Tools;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Celeste.UI.Input
{
    [AddComponentMenu("Celeste/UI/Input/Draggable UI")]
    public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Properties and Fields

        [SerializeField] private Canvas parentCanvas;
        [SerializeField] private UnityEvent<Vector3> onEndDrag;

        private Vector2 lastDragPosition;
        private bool isDragging = false;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGetInParent(ref parentCanvas);
        }

        private void OnEnable()
        {
            isDragging = false;
        }

        #endregion

        #region IDrag Interfaces

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = eventData.pointerPressRaycast.gameObject == gameObject;

            if (isDragging)
            {
                lastDragPosition = eventData.position;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isDragging)
            {
                RectTransform parentCanvasRectTransform = parentCanvas.transform as RectTransform;

                // Calculate the drag delta in local space and use that to apply to our transform
                RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvasRectTransform, lastDragPosition, parentCanvas.worldCamera, out Vector2 lastPosition);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvasRectTransform, eventData.position, parentCanvas.worldCamera, out Vector2 currentPosition);
                transform.Translate((currentPosition - lastPosition).ToVector3());
                lastDragPosition = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;

            if (onEndDrag != null)
            {
                onEndDrag.Invoke(transform.localPosition);
            }
        }

        #endregion
    }
}