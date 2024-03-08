using Celeste.Events;
using Celeste.Input.Settings;
using Celeste.Parameters;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.UI;

// DO NOT DELETE THIS, IT IS NEEDED
using UnityEngine.InputSystem.EnhancedTouch;

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
                if (raycastCamera.Value != null)
                {
                    Vector2 viewportCoords = raycastCamera.Value.ScreenToViewportPoint(Mouse.current.position.ReadValue());
                    return viewportCoords.x >= 0 && viewportCoords.x <= 1 && viewportCoords.y >= 0 && viewportCoords.y <= 1;
                }

                return false;
            }
        }

#endif

        [SerializeField] private CameraValue raycastCamera = default;
        [SerializeField] private InputState inputState;
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private InputSystemUIInputModule uiInputModule;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
#if UNITY_EDITOR
            var inputSettings = InputEditorSettings.GetOrCreateSettings();

            if (inputSettings.InputCamera != null)
            {
                raycastCamera = inputSettings.InputCamera;
            }
#endif
        }

        private void OnEnable()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (!EnhancedTouchSupport.enabled)
            {
                EnhancedTouchSupport.Enable();
            }
#endif
        }

        private void OnDisable()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (EnhancedTouchSupport.enabled)
            {
                EnhancedTouchSupport.Disable();
            }
#endif
        }

        private void Update()
        {
#if UNITY_ANDROID || UNITY_IOS
            GameObject hitGameObject = null;
            var touches = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches;
            int numTouches = touches.Count;
            Vector3 touchPosition = Vector3.zero;
            Vector3 touchWorldPosition = Vector3.zero;

            if (numTouches == 1)
            {
                ValueTuple<Vector3, GameObject> hitObject = CalculateHitObjectAndWorldPosition(touchPosition);
                touchWorldPosition = hitObject.Item1;
                hitGameObject = hitObject.Item2;
            }

            inputState.UpdatePointerPosition(touchPosition, touchWorldPosition);
            inputState.UpdatePointerOverObject(hitGameObject, numTouches == 1);
            inputState.UpdateTouches(touches);
#else
            Mouse mouse = Mouse.current;
            Vector2 mousePosition = Mouse.current.position.ReadValue();

#if UNITY_EDITOR
            if (!EditorOnly_MouseOverGameView)
            {
                // Disable input events when mouse not over the game view and release any held input
                inputState.ReleaseInput();
                return;
            }
#endif
            ValueTuple<Vector3, GameObject> hitObject = CalculateHitObjectAndWorldPosition(mousePosition);
            inputState.UpdatePointerPosition(mousePosition, hitObject.Item1);
            inputState.UpdatePointerOverObject(hitObject.Item2, mouse.leftButton.isPressed);

            CheckMouseButton(mouse.leftButton, MouseButton.Left);
            CheckMouseButton(mouse.rightButton, MouseButton.Right);
            CheckMouseButton(mouse.middleButton, MouseButton.Middle);

            float mouseScrollDelta = mouse.scroll.ReadValue().y;
            inputState.UpdateMouseScroll(mouseScrollDelta);
#endif
        }

        #endregion

        #region Utility Functions

        private ValueTuple<Vector3, GameObject> CalculateHitObjectAndWorldPosition(Vector2 pointerPosition)
        {
            Vector3 pointerWorldPosition = Vector3.zero;
            GameObject hitGameObject = null;

            if (raycastCamera.Value != null)
            {
                if (raycastCamera.Value != null)
                {
                    pointerWorldPosition = raycastCamera.Value.ScreenToWorldPoint(pointerPosition);

                    if (eventSystem.IsPointerOverGameObject() && uiInputModule != null)
                    {
                        hitGameObject = uiInputModule.GetLastRaycastResult(0).gameObject;
                        Debug.Log($"Hit UI Game Object {(hitGameObject != null ? hitGameObject.name : "none")}");
                    }

                    if (hitGameObject == null)
                    {
                        // If we haven't hit any UI, see if we have hit any game objects in the world
                        hitGameObject = Raycast(new Vector2(pointerWorldPosition.x, pointerWorldPosition.y));
                        Debug.Log($"Hit Game Object {(hitGameObject != null ? hitGameObject.name : "none")}");
                    }
                }
            }

            return new ValueTuple<Vector3, GameObject>(pointerWorldPosition, hitGameObject);
        }

        private void CheckMouseButton(ButtonControl buttonControl, MouseButton mouseButton)
        {
            PointerState mouseButtonState = new PointerState()
            {
                wasFirstDownThisFrame = buttonControl.wasPressedThisFrame,
                isDown = buttonControl.isPressed,
                wasFirstUpThisFrame= buttonControl.wasReleasedThisFrame,
            };
        
            inputState.UpdateMouseButtonState(mouseButton, mouseButtonState);
        }

        private GameObject Raycast(Vector2 origin)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(origin, Vector2.zero);
            return raycastHit.transform != null ? raycastHit.transform.gameObject : null;
        }

        #endregion
    }
}
