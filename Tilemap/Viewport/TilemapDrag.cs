using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Tilemaps
{
    [AddComponentMenu("Celeste/Tilemaps/Tilemap Drag")]
    [RequireComponent(typeof(Camera))]
    public class TilemapDrag : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private TilemapValue tilemap;
        [SerializeField] private FloatValue dragSpeed;

        private Camera cameraToDrag;
        private float timeSinceFingerDown = 0;
        private const float DRAG_THRESHOLD = 0.1f;
        private bool mouseDownLastFrame = false;
        private Vector2 previousMouseDownPosition = new Vector2();

        #endregion

        #region Unity Methods

        private void Start()
        {
            cameraToDrag = GetComponent<Camera>();
        }

        #endregion

        #region Utility Functions

        public void DragUsingMouse(Vector2 mousePosition)
        {
            if (mouseDownLastFrame)
            {
                Vector3 mouseDelta = previousMouseDownPosition - mousePosition;
                float scrollModifier = dragSpeed.Value * cameraToDrag.orthographicSize;

                transform.Translate(mouseDelta.x * scrollModifier, mouseDelta.y * scrollModifier, 0);
                ClampCamera();
            }

            previousMouseDownPosition = mousePosition;
            mouseDownLastFrame = true;
        }

        public void EndDrag()
        {
            mouseDownLastFrame = false;
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
                        Vector2 dragAmount = -touch.delta;
                        float scrollModifier = dragSpeed.Value * cameraToDrag.orthographicSize;

                        transform.Translate(dragAmount.x * scrollModifier, dragAmount.y * scrollModifier, 0);
                        ClampCamera();
                    }
                    break;

                default:
                    timeSinceFingerDown = 0;
                    break;
            }
        }

        private void ClampCamera()
        {
            Bounds bounds = tilemap.Value.localBounds;
            Vector3 currentPosition = transform.position;
            currentPosition.x = Mathf.Clamp(currentPosition.x, bounds.min.x, bounds.max.x);
            currentPosition.y = Mathf.Clamp(currentPosition.y, bounds.min.y, bounds.max.y);
            transform.position = currentPosition;
        }

        #endregion
    }
}
