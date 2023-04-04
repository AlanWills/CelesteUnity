using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Viewport
{
    [AddComponentMenu("Celeste/Viewport/Drag Camera")]
    public class DragCamera : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Camera cameraToDrag;
        [SerializeField] private Transform transformToMove;
        [SerializeField] private FloatReference dragSpeed;

        private float timeSinceFingerDown = 0;
        private const float DRAG_THRESHOLD = 0.1f;
        private bool dragStarted = false;
        private Vector2 previousMouseDownPosition = new Vector2();

        #endregion

        #region Unity Methods

        private void Awake()
        {
            CreateDragSpeedIfNotSet();
        }

        private void OnValidate()
        {
            if (cameraToDrag == null)
            {
                cameraToDrag = GetComponent<Camera>();
            }

            if (transformToMove == null && cameraToDrag != null)
            {
                transformToMove = cameraToDrag.transform;
            }

            CreateDragSpeedIfNotSet();
        }

        #endregion

        #region Utility Functions

        public void StartDrag(Vector2 mousePosition)
        {
            dragStarted = true;
            previousMouseDownPosition = mousePosition;
        }

        public void DragUsingMouse(Vector2 mousePosition)
        {
            if (dragStarted)
            {
                Vector3 previousMouseDownWorldPosition = cameraToDrag.ScreenToWorldPoint(previousMouseDownPosition);
                Vector3 mouseWorldPosition = cameraToDrag.ScreenToWorldPoint(mousePosition);
                Vector2 mouseDelta = mouseWorldPosition - previousMouseDownWorldPosition;
                mouseDelta *= dragSpeed.Value;

                transformToMove.position += new Vector3(mouseDelta.x, mouseDelta.y, 0);
                previousMouseDownPosition = mousePosition;
            }
        }

        public void EndDrag()
        {
            dragStarted = false;
        }

        public void DragUsingTouch(UnityEngine.InputSystem.EnhancedTouch.Touch touch)
        {
            switch (touch.phase)
            {
                case UnityEngine.InputSystem.TouchPhase.Began:
                    timeSinceFingerDown = 0;
                    break;

                case UnityEngine.InputSystem.TouchPhase.Stationary:
                    timeSinceFingerDown += Time.deltaTime;
                    break;

                case UnityEngine.InputSystem.TouchPhase.Moved:
                    timeSinceFingerDown += Time.deltaTime;

                    if (timeSinceFingerDown >= DRAG_THRESHOLD)
                    {
                        Vector2 touchPosition = touch.screenPosition;
                        Vector3 previousTouchDownWorldPosition = cameraToDrag.ScreenToWorldPoint(touchPosition - touch.delta);
                        Vector3 touchWorldPosition = cameraToDrag.ScreenToWorldPoint(touchPosition);
                        Vector2 dragAmount = touchWorldPosition - previousTouchDownWorldPosition;
                        float scrollModifier = dragSpeed.Value;

                        transformToMove.Translate(dragAmount.x * scrollModifier, dragAmount.y * scrollModifier, 0);
                    }
                    break;

                default:
                    timeSinceFingerDown = 0;
                    break;
            }
        }

        private void CreateDragSpeedIfNotSet()
        {
            if (dragSpeed == null)
            {
                dragSpeed = ScriptableObject.CreateInstance<FloatReference>();
                dragSpeed.IsConstant = true;
                dragSpeed.Value = 1;
            }
        }

        #endregion
    }
}
