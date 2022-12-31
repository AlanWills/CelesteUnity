using Celeste.Events;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.UI;

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
                Vector2 viewportCoords = raycastCamera.Value.ScreenToViewportPoint(Mouse.current.position.ReadValue());
                return viewportCoords.x >= 0 && viewportCoords.x <= 1 && viewportCoords.y >= 0 && viewportCoords.y <= 1;
            }
        }

#endif

        public CameraValue raycastCamera = default;

        [SerializeField] private InputState inputState;
        [SerializeField] private EventSystem eventSystem;

#region Desktop Variables

        [Header("Desktop Events")]
        public Vector2Event leftMouseButtonFirstDown;
        public Vector2Event leftMouseButtonDown;
        public Vector2Event leftMouseButtonFirstUp;

        public Vector2Event middleMouseButtonFirstDown;
        public Vector2Event middleMouseButtonDown;
        public Vector2Event middleMouseButtonFirstUp;

        public Vector2Event rightMouseButtonFirstDown;
        public Vector2Event rightMouseButtonDown;
        public Vector2Event rightMouseButtonFirstUp;

        public FloatEvent mouseScrolled;
        public Vector2Event mouseMoved;

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

        [NonSerialized] private List<RaycastResult> uiRaycastResults = new List<RaycastResult>();

        #endregion

        #region Unity Methods

        private void Update()
        {
#if UNITY_ANDROID || UNITY_IOS
            //if (UnityEngine.Input.touchCount != 1)
            //{
            //    firstHitGameObject = null;
            //}

            //if (UnityEngine.Input.touchCount == 1)
            //{
            //    //Touch touch = UnityEngine.Input.GetTouch(0);
            //    //singleTouchEvent.InvokeSilently(touch);

            //    //Vector3 touchWorldPosition = raycastCamera.ScreenToWorldPoint(touch.position);
            //    //GameObject hitGameObject = Raycast(new Vector2(touchWorldPosition.x, touchWorldPosition.y));

            //    //if (touch.phase == TouchPhase.Began)
            //    //{
            //    //    firstHitGameObject = hitGameObject;
            //    //    firstHitGameObjectPosition = touchWorldPosition;
            //    //}
            //    //else if (touch.phase == TouchPhase.Ended)
            //    //{
            //    //    if (firstHitGameObject != null && 
            //    //        firstHitGameObject == hitGameObject && 
            //    //        (touchWorldPosition - firstHitGameObjectPosition).sqrMagnitude < 0.05f)
            //    //    {
            //    //        gameObjectLeftClicked.Raise(new GameObjectClickEventArgs()
            //    //        {
            //    //            gameObject = hitGameObject,
            //    //            clickWorldPosition = touchWorldPosition
            //    //        });
            //    //    }

            //    //    firstHitGameObject = null;
            //    //}
            //}
            //else if (UnityEngine.Input.touchCount == 2)
            //{
            //    doubleTouchEvent.InvokeSilently(new MultiTouchEventArgs()
            //    {
            //        touchCount = 2,
            //        touches = UnityEngine.Input.touches,
            //    });
            //}
            //else if (UnityEngine.Input.touchCount == 3)
            //{
            //    tripleTouchEvent.InvokeSilently(new MultiTouchEventArgs()
            //    {
            //        touchCount = 3,
            //        touches = UnityEngine.Input.touches,
            //    });
            //}
#else

#if UNITY_EDITOR
            if (!EditorOnly_MouseOverGameView)
            {
                // Disable input events when mouse not over the game view
                return;
            }
#endif
            Mouse mouse = Mouse.current;
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = raycastCamera.Value.ScreenToWorldPoint(mousePosition);

            inputState.MousePosition = mousePosition;
            inputState.MouseWorldPosition = mouseWorldPosition;
            
            GameObject hitGameObject = null;
            
            if (raycastCamera.Value != null && !eventSystem.IsPointerOverGameObject())
            {
                // If we haven't hit any UI, see if we have hit any game objects in the world
                hitGameObject = Raycast(new Vector2(mouseWorldPosition.x, mouseWorldPosition.y));
                Debug.Log($"Hit Game Object {(hitGameObject != null ? hitGameObject.name : "none")}");
            }
            inputState.HitGameObject = hitGameObject;

            CheckMouseButton(
                mousePosition, 
                hitGameObject, 
                mouse.leftButton,
                MouseButton.Left,
                leftMouseButtonFirstDown, 
                leftMouseButtonDown, 
                leftMouseButtonFirstUp, 
                gameObjectLeftClicked);

            CheckMouseButton(
                mousePosition,
                hitGameObject,
                mouse.rightButton,
                MouseButton.Right,
                rightMouseButtonFirstDown, 
                rightMouseButtonDown, 
                rightMouseButtonFirstUp, 
                gameObjectRightClicked);

            CheckMouseButton(
                mousePosition, 
                hitGameObject, 
                mouse.middleButton,
                MouseButton.Middle,
                middleMouseButtonFirstDown, 
                middleMouseButtonDown, 
                middleMouseButtonFirstUp, 
                gameObjectMiddleClicked);

            float mouseScrollDelta = mouse.scroll.ReadValue().y;
            if (mouseScrollDelta != 0)
            {
                mouseScrolled.InvokeSilently(mouseScrollDelta);
            }

            mouseMoved.InvokeSilently(mousePosition);
#endif
        }

#endregion

        #region Utility Functions

        private void CheckMouseButton(
            Vector2 mousePosition,
            GameObject hitGameObject,
            ButtonControl buttonControl,
            MouseButton mouseButton,
            Vector2Event mouseButtonFirstDown,
            Vector2Event mouseButtonDown,
            Vector2Event mouseButtonFirstUpEvent,
            GameObjectClickEvent gameObjectClickedEvent)
        {
            if (eventSystem.currentSelectedGameObject != null)
            {
                // We check for UI interactions before registering mouse down events
                // Interacting with a UI element, so nothing should fire
                return;
            }

            MouseButtonState mouseButtonState = new MouseButtonState()
            {
                wasPressedThisFrame = buttonControl.wasPressedThisFrame,
                isPressed = buttonControl.isPressed,
                wasReleasedThisFrame= buttonControl.wasReleasedThisFrame,
            };
        
            if (mouseButtonState.wasPressedThisFrame)
            {
                mouseButtonFirstDown.InvokeSilently(mousePosition);
                
                if (hitGameObject != null)
                {
                    gameObjectClickedEvent.Invoke(new GameObjectClickEventArgs()
                    {
                        gameObject = hitGameObject,
                        clickWorldPosition = raycastCamera.Value.ScreenToWorldPoint(mousePosition)
                    });
                }
            }

            if (mouseButtonState.isPressed)
            {
                mouseButtonDown.InvokeSilently(mousePosition);
            }

            if (mouseButtonState.wasReleasedThisFrame)
            {
                mouseButtonFirstUpEvent.InvokeSilently(mousePosition);
            }

            inputState.SetMouseButton(mouseButton, mouseButtonState);
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
