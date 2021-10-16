using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Viewport
{
    [AddComponentMenu("Celeste/Viewport/Drag Camera")]
    public class DragCamera : MonoBehaviour
    {
        #region Properties and Fields

        public float CameraSizeModifier
        {
            get { return cameraToDrag.orthographic ? cameraToDrag.orthographicSize : -cameraToDrag.transform.position.z; }
        }

        [SerializeField] private Camera cameraToDrag;
        [SerializeField] private FloatReference dragSpeed;

        private float timeSinceFingerDown = 0;
        private const float DRAG_THRESHOLD = 0.1f;
        private bool mouseDownLastFrame = false;
        private Vector3 previousMouseDownPosition = new Vector3();
        private Vector2 dragDelta = new Vector2();

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

            CreateDragSpeedIfNotSet();
        }

        private void LateUpdate()
        {
            if (dragDelta != Vector2.zero)
            {
                transform.Translate(dragDelta.x, dragDelta.y, 0, Space.Self);
                dragDelta = Vector2.zero;
            }
        }

        #endregion

        #region Utility Functions

        public void DragUsingMouse(Vector3 mousePosition)
        {
            if (mouseDownLastFrame)
            {
                Vector3 mouseDelta = cameraToDrag.ScreenToViewportPoint(previousMouseDownPosition) - cameraToDrag.ScreenToViewportPoint(Input.mousePosition);
                float scrollModifier = dragSpeed.Value * CameraSizeModifier;
                dragDelta.x = mouseDelta.x * scrollModifier;
                dragDelta.y = mouseDelta.y * scrollModifier;
            }

            previousMouseDownPosition = Input.mousePosition;
            mouseDownLastFrame = true;
        }

        public void EndDrag()
        {
            mouseDownLastFrame = false;
            dragDelta = Vector2.zero;
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
                        float scrollModifier = dragSpeed.Value * Time.deltaTime * CameraSizeModifier;

                        transform.Translate(dragAmount.x * scrollModifier, dragAmount.y * scrollModifier, 0);
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
