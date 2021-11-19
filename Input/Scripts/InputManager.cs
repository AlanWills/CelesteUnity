using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Celeste.Input
{
    [AddComponentMenu("Celeste/Input/Input Manager")]
    public class InputManager : MonoBehaviour
    {
        #region Properties and Fields

#if UNITY_EDITOR

        public bool EditorOnly_MouseOverGameView
        {
            get
            {
                Vector2 viewportCoords = raycastCamera.Value.ScreenToViewportPoint(UnityEngine.Input.mousePosition);
                return viewportCoords.x >= 0 && viewportCoords.x <= 1 && viewportCoords.y >= 0 && viewportCoords.y <= 1;
            }
        }

#endif

        public CameraValue raycastCamera = default;

        [SerializeField] private EventSystem eventSystem;

#region Desktop Variables

        [Header("Desktop Events")]
        public Vector3Event leftMouseButtonFirstDown;
        public Vector3Event leftMouseButtonDown;
        public Vector3Event leftMouseButtonFirstUp;

        public Vector3Event middleMouseButtonFirstDown;
        public Vector3Event middleMouseButtonDown;
        public Vector3Event middleMouseButtonFirstUp;

        public Vector3Event rightMouseButtonFirstDown;
        public Vector3Event rightMouseButtonDown;
        public Vector3Event rightMouseButtonFirstUp;

        public FloatEvent mouseScrolled;
        public Vector3Event mouseMoved;

#endregion

#region Phone Variables

        [Header("Phone Events")]
        public TouchEvent singleTouchEvent;
        public MultiTouchEvent doubleTouchEvent;
        public MultiTouchEvent tripleTouchEvent;

#endregion

#region Common Variables

        [Header("Common Events")]
        public GameObjectClickEvent gameObjectLeftClicked;
        public GameObjectClickEvent gameObjectMiddleClicked;
        public GameObjectClickEvent gameObjectRightClicked;

        #endregion

        #endregion

        #region Unity Methods

        private void Update()
        {
            if (raycastCamera.Value == null)
            {
                return;
            }

#if UNITY_ANDROID || UNITY_IOS
            //if (UnityEngine.Input.touchCount != 1)
            //{
            //    firstHitGameObject = null;
            //}

            if (UnityEngine.Input.touchCount == 1)
            {
                Touch touch = UnityEngine.Input.GetTouch(0);
                singleTouchEvent.InvokeSilently(touch);

                //Vector3 touchWorldPosition = raycastCamera.ScreenToWorldPoint(touch.position);
                //GameObject hitGameObject = Raycast(new Vector2(touchWorldPosition.x, touchWorldPosition.y));

                //if (touch.phase == TouchPhase.Began)
                //{
                //    firstHitGameObject = hitGameObject;
                //    firstHitGameObjectPosition = touchWorldPosition;
                //}
                //else if (touch.phase == TouchPhase.Ended)
                //{
                //    if (firstHitGameObject != null && 
                //        firstHitGameObject == hitGameObject && 
                //        (touchWorldPosition - firstHitGameObjectPosition).sqrMagnitude < 0.05f)
                //    {
                //        gameObjectLeftClicked.Raise(new GameObjectClickEventArgs()
                //        {
                //            gameObject = hitGameObject,
                //            clickWorldPosition = touchWorldPosition
                //        });
                //    }

                //    firstHitGameObject = null;
                //}
            }
            else if (UnityEngine.Input.touchCount == 2)
            {
                doubleTouchEvent.InvokeSilently(new MultiTouchEventArgs()
                {
                    touchCount = 2,
                    touches = UnityEngine.Input.touches,
                });
            }
            else if (UnityEngine.Input.touchCount == 3)
            {
                tripleTouchEvent.InvokeSilently(new MultiTouchEventArgs()
                {
                    touchCount = 3,
                    touches = UnityEngine.Input.touches,
                });
            }
#else

#if UNITY_EDITOR
            if (!EditorOnly_MouseOverGameView)
            {
                // Disable input events when mouse not over the game view
                return;
            }
#endif

            CheckMouseButton(MouseButton.LeftMouse, leftMouseButtonFirstDown, leftMouseButtonDown, leftMouseButtonFirstUp, gameObjectLeftClicked);
            CheckMouseButton(MouseButton.RightMouse, rightMouseButtonFirstDown, rightMouseButtonDown, rightMouseButtonFirstUp, gameObjectRightClicked);
            CheckMouseButton(MouseButton.MiddleMouse, middleMouseButtonFirstDown, middleMouseButtonDown, middleMouseButtonFirstUp, gameObjectMiddleClicked);

            float mouseScrollDelta = UnityEngine.Input.mouseScrollDelta.y;
            if (mouseScrollDelta != 0)
            {
                mouseScrolled.InvokeSilently(mouseScrollDelta);
            }

            Vector3 mousePosition = UnityEngine.Input.mousePosition;
            mousePosition.z = -raycastCamera.Value.transform.position.z;
            Vector3 mouseWorldPosition = raycastCamera.Value.ScreenToWorldPoint(mousePosition);
            mouseMoved.InvokeSilently(mouseWorldPosition);
#endif
        }

#endregion

        #region Utility Functions

        private void CheckMouseButton(
            MouseButton mouseButton, 
            Vector3Event mouseButtonFirstDown,
            Vector3Event mouseButtonDown,
            Vector3Event mouseButtonFirstUpEvent,
            GameObjectClickEvent gameObjectClickedEvent)
        {
            if (eventSystem.currentSelectedGameObject != null)
            {
                // We check for UI interactions before registering mouse down events
                // Interacting with a UI element, so nothing should fire
                return;
            }
        
            Vector3 mousePosition = UnityEngine.Input.mousePosition;
            mousePosition.z = -raycastCamera.Value.transform.position.z;
            Vector3 clickWorldPosition = raycastCamera.Value.ScreenToWorldPoint(mousePosition);

            if (UnityEngine.Input.GetMouseButtonDown((int)mouseButton))
            {
                mouseButtonFirstDown.InvokeSilently(clickWorldPosition);

                GameObject hitGameObject = Raycast(new Vector2(clickWorldPosition.x, clickWorldPosition.y));
                if (hitGameObject != null)
                {
                    gameObjectClickedEvent.Invoke(new GameObjectClickEventArgs()
                    {
                        gameObject = hitGameObject,
                        clickWorldPosition = clickWorldPosition
                    });
                }
            }

            if (UnityEngine.Input.GetMouseButton((int)mouseButton))
            {
                mouseButtonDown.InvokeSilently(clickWorldPosition);
            }

            if (UnityEngine.Input.GetMouseButtonUp((int)mouseButton))
            {
                mouseButtonFirstUpEvent.InvokeSilently(clickWorldPosition);
            }
        }

        #endregion

        #region Raycasting

        private GameObject Raycast(Vector2 origin)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(origin, Vector2.zero);
            return raycastHit.transform != null ? raycastHit.transform.gameObject : null;
        }

        #endregion
    }
}
