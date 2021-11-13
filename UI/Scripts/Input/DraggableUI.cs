using Celeste.Maths;
using Celeste.Tools;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Celeste.UI.Input
{
    [AddComponentMenu("Celeste/UI/Input/Draggable UI")]
    public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Properties and Fields

        [SerializeField] private Canvas parentCanvas;

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
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isDragging)
            {
                RectTransform parentCanvasRectTransform = parentCanvas.transform as RectTransform;
                
                if (parentCanvasRectTransform == transform.parent)
                {
                    // For direct children, we just translate
                    transform.Translate(eventData.delta.ToVector3());
                }
                else
                {
                    // Whereas for sub-children we must perform this relative calculation
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvasRectTransform, eventData.position, parentCanvas.worldCamera, out Vector2 pos);
                    transform.position = parentCanvasRectTransform.TransformPoint(pos);
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
        }

        #endregion
    }
}