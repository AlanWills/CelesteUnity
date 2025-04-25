using Celeste.Events;
using Celeste.Input.Settings;
using Celeste.Parameters;
using Celeste.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
#if USE_NEW_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Utilities;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
#else
using Touch = UnityEngine.Touch;
#endif

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

        public bool VerboseLogging { get; set; }
        public Camera RaycastCamera
        {
            get => raycastCamera.Value ?? fallbackMainCamera;
            set
            {
                raycastCamera.Value = value;
            }
        }

        public Vector2 PointerPosition { get; private set; }
        public Vector3 PointerWorldPosition { get; private set; }
        public PointerState PointerState { get; private set; }
        public Vector2 PreviousPointerPosition { get; private set; }
        public Vector3 PreviousPointerWorldPosition { get; private set; }
        public PointerState PreviousPointerState { get; private set; }

        public GameObject HitGameObject { get; private set; }
        public GameObject PreviousHitGameObject { get; private set; }

        public int NumTouches => Touches.Count;
        public IReadOnlyList<Touch> Touches { get; private set; }

        public PointerState LeftMouseButton { get; private set; }
        public PointerState MiddleMouseButton { get; private set; }
        public PointerState RightMouseButton { get; private set; }
        public float MouseScroll { get; private set; }

        [Header("Data")]
        [SerializeField] private bool defaultVerboseLogging = false;
        [SerializeField] private CameraValue raycastCamera = default;

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
        
        [NonSerialized] private Camera fallbackMainCamera;
        [NonSerialized] private List<RaycastResult> eventSystemRaycastResults = new();

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
#if UNITY_EDITOR
            var inputSettings = InputEditorSettings.GetOrCreateSettings();
            
            if (inputSettings.InputCamera != null)
            {
                raycastCamera = inputSettings.InputCamera;
                EditorOnly.SetDirty(this);
            }

            if (inputSettings.LeftMouseButtonFirstDown != null && leftMouseButtonFirstDown == null)
            {
                leftMouseButtonFirstDown = inputSettings.LeftMouseButtonFirstDown;
                EditorOnly.SetDirty(this);
            }

            if (inputSettings.LeftMouseButtonDown != null && leftMouseButtonDown == null)
            {
                leftMouseButtonDown = inputSettings.LeftMouseButtonDown;
                EditorOnly.SetDirty(this);
            }

            if (inputSettings.LeftMouseButtonFirstUp != null && leftMouseButtonFirstUp == null)
            {
                leftMouseButtonFirstUp = inputSettings.LeftMouseButtonFirstUp;
                EditorOnly.SetDirty(this);
            }

            if (inputSettings.MiddleMouseButtonFirstDown != null && middleMouseButtonFirstDown == null)
            {
                middleMouseButtonFirstDown = inputSettings.MiddleMouseButtonFirstDown;
                EditorOnly.SetDirty(this);
            }

            if (inputSettings.MiddleMouseButtonDown != null && middleMouseButtonDown == null)
            {
                middleMouseButtonDown = inputSettings.MiddleMouseButtonDown;
                EditorOnly.SetDirty(this);
            }

            if (inputSettings.MiddleMouseButtonFirstUp != null && middleMouseButtonFirstUp == null)
            {
                middleMouseButtonFirstUp = inputSettings.MiddleMouseButtonFirstUp;
                EditorOnly.SetDirty(this);
            }

            if (inputSettings.RightMouseButtonFirstDown != null && rightMouseButtonFirstDown == null)
            {
                rightMouseButtonFirstDown = inputSettings.RightMouseButtonFirstDown;
                EditorOnly.SetDirty(this);
            }

            if (inputSettings.RightMouseButtonDown != null && rightMouseButtonDown == null)
            {
                rightMouseButtonDown = inputSettings.RightMouseButtonDown;
                EditorOnly.SetDirty(this);
            }

            if (inputSettings.RightMouseButtonFirstUp != null && rightMouseButtonFirstUp == null)
            {
                rightMouseButtonFirstUp = inputSettings.RightMouseButtonFirstUp;
                EditorOnly.SetDirty(this);
            }

            if (inputSettings.SingleTouch != null && singleTouch == null)
            {
                singleTouch = inputSettings.SingleTouch;
                EditorOnly.SetDirty(this);
            }

            if (inputSettings.DoubleTouch != null && doubleTouch == null)
            {
                doubleTouch = inputSettings.DoubleTouch;
                EditorOnly.SetDirty(this);
            }

            if (inputSettings.TripleTouch != null && tripleTouch == null)
            {
                tripleTouch = inputSettings.TripleTouch;
                EditorOnly.SetDirty(this);
            }
#endif
        }

#endregion

        public void Initialize()
        {
            VerboseLogging = defaultVerboseLogging;
            fallbackMainCamera = null;
        }

        public void CheckRaycastCamera()
        {
            if (raycastCamera.Value == null && fallbackMainCamera == null)
            {
                UnityEngine.Debug.LogWarning($"No raycast camera found, falling back to main camera to try and provide some input support.", CelesteLog.Input.WithContext(this));
                fallbackMainCamera = Camera.main;
            }
        }

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
                    pointerExitedGameObjectEvent.Invoke(PreviousHitGameObject, VerboseLogging);
                }

                if (newHitGameObject != null)
                {
                    newInputHandler?.OnPointerEnter(this);
                    pointerEnteredGameObjectEvent.Invoke(newHitGameObject, VerboseLogging);
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
                    pointerFirstDownOnGameObjectEvent?.Invoke(newHitGameObject, VerboseLogging);
                }

                if (PointerState.isDown)
                {
                    newInputHandler?.OnPointerDown(this);
                    pointerDownOnGameObjectEvent?.Invoke(newHitGameObject, VerboseLogging);
                }

                if (PointerState.wasFirstUpThisFrame)
                {
                    newInputHandler?.OnPointerFirstUp(this);
                    pointerFirstUpFromGameObjectEvent?.Invoke(newHitGameObject, VerboseLogging);
                }
            }
        }

        public void UpdateTouches(IReadOnlyList<Touch> touches)
        {
            int numTouches = touches.Count;
            Touches = touches;

            if (numTouches == 1)
            {
                UnityEngine.Debug.Assert(singleTouch != null, $"No {nameof(singleTouch)} event found on {nameof(InputState)} {name}.", CelesteLog.Input.WithContext(this));
                singleTouch.Invoke(touches[0], VerboseLogging);
            }
            else if (numTouches == 2)
            {
                UnityEngine.Debug.Assert(doubleTouch != null, $"No {nameof(doubleTouch)} event found on {nameof(InputState)} {name}.", CelesteLog.Input.WithContext(this));
                doubleTouch.Invoke(new MultiTouchEventArgs()
                {
                    touchCount = numTouches,
                    touches = touches,
                }, VerboseLogging);
            }
            else if (numTouches == 3)
            {
                UnityEngine.Debug.Assert(tripleTouch != null, $"No {nameof(tripleTouch)} event found on {nameof(InputState)} {name}.", CelesteLog.Input.WithContext(this));
                tripleTouch.Invoke(new MultiTouchEventArgs()
                {
                    touchCount = numTouches,
                    touches = touches,
                }, VerboseLogging);
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

        [Conditional("DEVELOPMENT")]
        private void VerboseLog(string message, UnityEngine.Object context)
        {
            if (VerboseLogging)
            {
                UnityEngine.Debug.Log(message, CelesteLog.Input.WithContext(context));
            }
        }

#if USE_NEW_INPUT_SYSTEM
        public ValueTuple<Vector3, GameObject> CalculateHitObjectAndWorldPosition(
            Vector2 pointerPosition, 
            int pointerOrTouchId,
            EventSystem eventSystem,
            InputSystemUIInputModule uiInputModule)
        {
            Vector3 pointerWorldPosition = Vector3.zero;
            GameObject hitGameObject = null;

            if (RaycastCamera != null)
            {
                pointerWorldPosition = RaycastCamera.ScreenToWorldPoint(pointerPosition);

                if (eventSystem.IsPointerOverGameObject() && uiInputModule != null)
                {
                    hitGameObject = uiInputModule.GetLastRaycastResult(pointerOrTouchId).gameObject;
                    VerboseLog($"Hit UI Game Object {(hitGameObject != null ? hitGameObject.name : "none")}", hitGameObject);
                }

                if (hitGameObject == null)
                {
                    // If we haven't hit any UI, see if we have hit any game objects in the world
                    hitGameObject = Raycast(new Vector2(pointerWorldPosition.x, pointerWorldPosition.y));
                    VerboseLog($"Hit Game Object {(hitGameObject != null ? hitGameObject.name : "none")}", hitGameObject);
                }
            }

            return new ValueTuple<Vector3, GameObject>(pointerWorldPosition, hitGameObject);
        }
#else
        public ValueTuple<Vector3, GameObject> CalculateHitObjectAndWorldPosition(
            Vector3 pointerPosition,
            EventSystem eventSystem)
        {
            Vector3 pointerWorldPosition = Vector3.zero;
            GameObject hitGameObject = null;

            if (RaycastCamera != null)
            {
                pointerWorldPosition = RaycastCamera.ScreenToWorldPoint(pointerPosition);

                if (eventSystem.IsPointerOverGameObject())
                {
                    hitGameObject = Raycast(pointerPosition, eventSystem);
                    VerboseLog($"Hit UI Game Object {(hitGameObject != null ? hitGameObject.name : "none")}", hitGameObject);
                }

                if (hitGameObject == null)
                {
                    // If we haven't hit any UI, see if we have hit any game objects in the world
                    hitGameObject = Raycast(new Vector2(pointerWorldPosition.x, pointerWorldPosition.y));
                    VerboseLog($"Hit Game Object {(hitGameObject != null ? hitGameObject.name : "none")}", hitGameObject);
                }
            }

            return new ValueTuple<Vector3, GameObject>(pointerWorldPosition, hitGameObject);
        }
#endif

        private GameObject Raycast(Vector2 origin)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(origin, Vector2.zero);
            return raycastHit.transform != null ? raycastHit.transform.gameObject : null;
        }

        private GameObject Raycast(Vector2 rayOrigin, EventSystem eventSystem)
        {
            eventSystemRaycastResults.Clear();
            
            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = rayOrigin
            };

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            eventSystem.RaycastAll(pointerData, raycastResults);

            return raycastResults.Count > 0 ? raycastResults[0].gameObject : null;
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
                firstDownEvent.Invoke(PointerPosition, VerboseLogging);

                if (HitGameObject != null)
                {
                    clickedEvent.Invoke(new GameObjectClickEventArgs()
                    {
                        gameObject = HitGameObject,
                        clickWorldPosition = PointerWorldPosition
                    }, VerboseLogging);
                }
            }

            if (mouseButtonState.isDown)
            {
                downEvent.InvokeSilently(PointerPosition);
            }

            if (mouseButtonState.wasFirstUpThisFrame)
            {
                firstUpEvent.Invoke(PointerPosition, VerboseLogging);
            }
        }
    }
}
