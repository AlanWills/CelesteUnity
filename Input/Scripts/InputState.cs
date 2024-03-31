using Celeste.Events;
using Celeste.Input.Settings;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.Utilities;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Celeste.Input
{
    [Serializable]
    public struct PointerState
    {
        public bool wasFirstDownThisFrame;
        public bool isDown;
        public bool wasFirstUpThisFrame;
    }

    [Serializable]
    public enum MouseButton
    {
        Left,
        Middle,
        Right,
    }

    [CreateAssetMenu(fileName = nameof(InputState), menuName = CelesteMenuItemConstants.INPUT_MENU_ITEM + "Input State", order = CelesteMenuItemConstants.INPUT_MENU_ITEM_PRIORITY)]
    public class InputState : ScriptableObject
    {
        #region Properties and Fields

        public Vector2 PointerPosition { get; private set; }
        public Vector3 PointerWorldPosition { get; private set; }
        public PointerState PointerState { get; private set; }
        public Vector2 PreviousPointerPosition { get; private set; }
        public Vector3 PreviousPointerWorldPosition { get; private set; }
        public PointerState PreviousPointerState { get; private set; }

        public GameObject HitGameObject { get; private set; }
        public GameObject PreviousHitGameObject { get; private set; }

        public int NumTouches => Touches.Count;
        public ReadOnlyArray<Touch> Touches { get; private set; }

        public PointerState LeftMouseButton { get; private set; }
        public PointerState MiddleMouseButton { get; private set; }
        public PointerState RightMouseButton { get; private set; }
        public float MouseScroll { get; private set; }

        [Header("Common Events")]
        [SerializeField] private Vector2Event pointerMoved;
        [SerializeField] private GameObjectEvent pointerEnteredGameObjectEvent;
        [SerializeField] private GameObjectEvent pointerExitedGameObjectEvent;
        [SerializeField] private GameObjectEvent pointerFirstDownOnGameObjectEvent;
        [SerializeField] private GameObjectEvent pointerDownOnGameObjectEvent;
        [SerializeField] private GameObjectEvent pointerFirstUpFromGameObjectEvent;

        [Header("Phone Events")]
        [SerializeField] private TouchEvent singleTouch;
        [SerializeField] private MultiTouchEvent doubleTouch;
        [SerializeField] private MultiTouchEvent tripleTouch;

        [Header("Desktop Events")]
        [SerializeField] private FloatEvent mouseScrolled;
        [SerializeField] private Vector2Event leftMouseButtonFirstDown;
        [SerializeField] private Vector2Event leftMouseButtonDown;
        [SerializeField] private Vector2Event leftMouseButtonFirstUp;
        [SerializeField] private Vector2Event middleMouseButtonFirstDown;
        [SerializeField] private Vector2Event middleMouseButtonDown;
        [SerializeField] private Vector2Event middleMouseButtonFirstUp;
        [SerializeField] private Vector2Event rightMouseButtonFirstDown;
        [SerializeField] private Vector2Event rightMouseButtonDown;
        [SerializeField] private Vector2Event rightMouseButtonFirstUp;
        [SerializeField] private GameObjectClickEvent gameObjectLeftClicked;
        [SerializeField] private GameObjectClickEvent gameObjectMiddleClicked;
        [SerializeField] private GameObjectClickEvent gameObjectRightClicked;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
#if UNITY_EDITOR
            var inputSettings = InputEditorSettings.GetOrCreateSettings();

            if (inputSettings.LeftMouseButtonFirstDown != null)
            {
                leftMouseButtonFirstDown = inputSettings.LeftMouseButtonFirstDown;
            }

            if (inputSettings.LeftMouseButtonDown != null)
            {
                leftMouseButtonDown = inputSettings.LeftMouseButtonDown;
            }

            if (inputSettings.LeftMouseButtonFirstUp != null)
            {
                leftMouseButtonFirstUp = inputSettings.LeftMouseButtonFirstUp;
            }

            if (inputSettings.MiddleMouseButtonFirstDown != null)
            {
                middleMouseButtonFirstDown = inputSettings.MiddleMouseButtonFirstDown;
            }

            if (inputSettings.MiddleMouseButtonDown != null)
            {
                middleMouseButtonDown = inputSettings.MiddleMouseButtonDown;
            }

            if (inputSettings.MiddleMouseButtonFirstUp != null)
            {
                middleMouseButtonFirstUp = inputSettings.MiddleMouseButtonFirstUp;
            }

            if (inputSettings.RightMouseButtonFirstDown != null)
            {
                rightMouseButtonFirstDown = inputSettings.RightMouseButtonFirstDown;
            }

            if (inputSettings.RightMouseButtonDown != null)
            {
                rightMouseButtonFirstDown = inputSettings.RightMouseButtonDown;
            }

            if (inputSettings.RightMouseButtonFirstUp != null)
            {
                rightMouseButtonFirstDown = inputSettings.RightMouseButtonFirstUp;
            }

            if (inputSettings.SingleTouch != null)
            {
                singleTouch = inputSettings.SingleTouch;
            }

            if (inputSettings.DoubleTouch != null)
            {
                doubleTouch = inputSettings.DoubleTouch;
            }

            if (inputSettings.TripleTouch != null)
            {
                tripleTouch = inputSettings.TripleTouch;
            }
#endif
        }

        #endregion

        public void UpdatePointerPosition(Vector2 position, Vector3 worldPosition)
        {
            PreviousPointerPosition = PointerPosition;
            PreviousPointerWorldPosition = PointerWorldPosition;

            if (PointerPosition != position)
            {
                PointerPosition = position;
                pointerMoved.InvokeSilently(position);
            }

            if (PointerWorldPosition != worldPosition)
            {
                PointerWorldPosition = worldPosition;
            }
        }

        public void UpdatePointerOverObject(GameObject newHitGameObject, bool isDownThisFrame)
        {
            // Update the state first, but don't fire events
            PreviousHitGameObject = HitGameObject;
            PreviousPointerState = PointerState;
            PointerState = new PointerState()
            {
                wasFirstDownThisFrame = !PreviousPointerState.isDown && isDownThisFrame,
                isDown = isDownThisFrame,
                wasFirstUpThisFrame = PreviousPointerState.isDown && !isDownThisFrame
            };
            HitGameObject = newHitGameObject;

            // Now take care of figuring out which events we need to fire

            IInputHandler currentInputHandler = PreviousHitGameObject != null ? PreviousHitGameObject.GetComponent<IInputHandler>() : null;
            IInputHandler newInputHandler = newHitGameObject != null ? newHitGameObject.GetComponent<IInputHandler>() : null;

            if (PreviousHitGameObject != newHitGameObject)
            {
                if (PreviousHitGameObject != null)
                {
                    currentInputHandler?.OnPointerExit(this);
                    pointerExitedGameObjectEvent.Invoke(PreviousHitGameObject);
                }

                if (newHitGameObject != null)
                {
                    newInputHandler?.OnPointerEnter(this);
                    pointerEnteredGameObjectEvent.Invoke(newHitGameObject);
                }
            }
            else if (PreviousHitGameObject != null)
            {
                currentInputHandler?.OnPointerOver(this);
            }

            // If the pointer is over something, we should tell it if we've pressed it or not
            if (newHitGameObject != null)
            {
                if (PointerState.wasFirstDownThisFrame)
                {
                    newInputHandler?.OnPointerFirstDown(this);
                    pointerFirstDownOnGameObjectEvent?.Invoke(newHitGameObject);
                }

                if (PointerState.isDown)
                {
                    newInputHandler?.OnPointerDown(this);
                    pointerDownOnGameObjectEvent?.Invoke(newHitGameObject);
                }

                if (PointerState.wasFirstUpThisFrame)
                {
                    newInputHandler?.OnPointerFirstUp(this);
                    pointerFirstUpFromGameObjectEvent?.Invoke(newHitGameObject);
                }
            }
        }

        public void UpdateTouches(ReadOnlyArray<Touch> touches)
        {
            int numTouches = touches.Count;
            Touches = touches;

            if (numTouches == 1)
            {
                singleTouch.Invoke(touches[0]);
            }
            else if (numTouches == 2)
            {
                doubleTouch.InvokeSilently(new MultiTouchEventArgs()
                {
                    touchCount = numTouches,
                    touches = touches,
                });
            }
            else if (numTouches == 3)
            {
                tripleTouch.InvokeSilently(new MultiTouchEventArgs()
                {
                    touchCount = numTouches,
                    touches = touches,
                });
            }
        }

        public void UpdateMouseButtonState(MouseButton mouseButton, PointerState mouseButtonState)
        {
            switch (mouseButton)
            {
                case MouseButton.Left:
                    LeftMouseButton = mouseButtonState;
                    CheckMouseEvents(
                        mouseButtonState, 
                        leftMouseButtonFirstDown, 
                        leftMouseButtonDown, 
                        leftMouseButtonFirstUp,
                        gameObjectLeftClicked);
                    break;

                case MouseButton.Middle:
                    MiddleMouseButton = mouseButtonState;
                    CheckMouseEvents(
                        mouseButtonState, 
                        middleMouseButtonFirstDown, 
                        middleMouseButtonDown, 
                        middleMouseButtonFirstUp,
                        gameObjectMiddleClicked);
                    break;

                case MouseButton.Right:
                    RightMouseButton = mouseButtonState;
                    CheckMouseEvents(
                        mouseButtonState, 
                        rightMouseButtonFirstDown, 
                        rightMouseButtonDown, 
                        rightMouseButtonFirstUp,
                        gameObjectRightClicked);
                    break;

                default:
                    break;
            }
        }

        public void UpdateMouseScroll(float scroll)
        {
            float oldScroll = MouseScroll;
            MouseScroll = oldScroll;

            if (scroll != 0 && scroll != oldScroll)
            {
                mouseScrolled.InvokeSilently(scroll);
            }
        }

        public void ReleaseInput()
        {
            PreviousPointerState = PointerState;
            PointerState = new PointerState()
            {
                wasFirstDownThisFrame = false,
                isDown = false,
                wasFirstUpThisFrame = true,
            };

            UpdateMouseButtonState(MouseButton.Left, PointerState);
            UpdateMouseButtonState(MouseButton.Middle, new PointerState()
            {
                wasFirstDownThisFrame = false,
                isDown = false,
                wasFirstUpThisFrame = true,
            });
            UpdateMouseButtonState(MouseButton.Right, new PointerState()
            {
                wasFirstDownThisFrame = false,
                isDown = false,
                wasFirstUpThisFrame = true,
            });
        }

        private void CheckMouseEvents(
            PointerState mouseButtonState, 
            Vector2Event firstDownEvent, 
            Vector2Event downEvent, 
            Vector2Event firstUpEvent,
            GameObjectClickEvent clickedEvent)
        {
            if (mouseButtonState.wasFirstDownThisFrame)
            {
                firstDownEvent.InvokeSilently(PointerPosition);

                if (HitGameObject != null)
                {
                    clickedEvent.Invoke(new GameObjectClickEventArgs()
                    {
                        gameObject = HitGameObject,
                        clickWorldPosition = PointerWorldPosition
                    });
                }
            }

            if (mouseButtonState.isDown)
            {
                downEvent.InvokeSilently(PointerPosition);
            }

            if (mouseButtonState.wasFirstUpThisFrame)
            {
                firstUpEvent.InvokeSilently(PointerPosition);
            }
        }
    }
}
