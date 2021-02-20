using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Viewport
{
    [AddComponentMenu("Celeste/Viewport/Drag")]
    [RequireComponent(typeof(Camera))]
    public class Drag : MonoBehaviour
    {
        #region Properties and Fields

        private Camera cameraToDrag;
        private FloatValue dragSpeed;

        private float timeSinceFingerDown = 0;
        private const float DRAG_THRESHOLD = 0.1f;
        private bool mouseDownLastFrame = false;
        private Vector3 previousMouseDownPosition = new Vector3();

        #endregion

        #region Unity Methods

        private void Start()
        {
            cameraToDrag = GetComponent<Camera>();
        }

        #endregion

        #region Utility Functions

        public void DragUsingMouse(Vector3 mousePosition)
        {
            if (mouseDownLastFrame)
            {
                Vector3 mouseDelta = previousMouseDownPosition - mousePosition;
                float scrollModifier = dragSpeed.Value * Time.deltaTime * cameraToDrag.orthographicSize;

                transform.Translate(mouseDelta.x * scrollModifier, mouseDelta.y * scrollModifier, 0);
            }

            previousMouseDownPosition = mousePosition;
            mouseDownLastFrame = true;
        }

        public void EndDrag()
        {
            mouseDownLastFrame = false;
        }

        public void DragUsingTouch(Touch touch)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    timeSinceFingerDown = 0;
                    break;

                case TouchPhase.Stationary:
                    timeSinceFingerDown += Time.deltaTime;
                    break;

                case TouchPhase.Moved:
                    timeSinceFingerDown += Time.deltaTime;

                    if (timeSinceFingerDown >= DRAG_THRESHOLD)
                    {
                        Vector2 dragAmount = -touch.deltaPosition;
                        float scrollModifier = dragSpeed.Value * Time.deltaTime * cameraToDrag.orthographicSize;

                        transform.Translate(dragAmount.x * scrollModifier, dragAmount.y * scrollModifier, 0);
                    }
                    break;

                default:
                    timeSinceFingerDown = 0;
                    break;
            }
        }

        #endregion
    }
}
