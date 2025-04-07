using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;
#if USE_NEW_INPUT_SYSTEM
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
#else
using Touch = UnityEngine.Touch;
#endif

namespace Celeste.Viewport
{
    [AddComponentMenu("Celeste/Viewport/Drag Camera")]
    public class DragCamera : MonoBehaviour
    {
        #region Properties and Fields

        public FloatReference DragSpeed => dragSpeed;

        [SerializeField] private Camera cameraToUse;
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

        public void OnValidate()
        {
            if (cameraToUse == null)
            {
                cameraToUse = GetComponent<Camera>();
            }

            if (transformToMove == null && cameraToUse != null)
            {
                transformToMove = cameraToUse.transform;
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
                Vector3 previousMouseDownWorldPosition = cameraToUse.ScreenToWorldPoint(previousMouseDownPosition);
                Vector3 mouseWorldPosition = cameraToUse.ScreenToWorldPoint(mousePosition);
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

        public void DragUsingTouch(Touch touch)
        {
#if USE_NEW_INPUT_SYSTEM
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
                        Vector3 previousTouchDownWorldPosition = cameraToUse.ScreenToWorldPoint(touchPosition - touch.delta);
                        Vector3 touchWorldPosition = cameraToUse.ScreenToWorldPoint(touchPosition);
                        Vector2 dragAmount = touchWorldPosition - previousTouchDownWorldPosition;
                        float scrollModifier = dragSpeed.Value;

                        transformToMove.Translate(dragAmount.x * scrollModifier, dragAmount.y * scrollModifier, 0);
                    }
                    break;

                default:
                    timeSinceFingerDown = 0;
                    break;
            }
#endif
        }
        
        public void DragUsingMultiTouch(MultiTouchEventArgs touchEventArgs)
        {
#if USE_NEW_INPUT_SYSTEM
            DragUsingTouch(touchEventArgs.touches[0]);
#endif
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
