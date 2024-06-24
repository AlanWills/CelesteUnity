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
                if (inputState.RaycastCamera != null)
                {
                    Vector2 viewportCoords = inputState.RaycastCamera.ScreenToViewportPoint(Mouse.current.position.ReadValue());
                    return viewportCoords.x >= 0 && viewportCoords.x <= 1 && viewportCoords.y >= 0 && viewportCoords.y <= 1;
                }

                return false;
            }
        }
#endif
        
        [SerializeField] private InputState inputState;
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private InputSystemUIInputModule uiInputModule;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            inputState.Initialize();
            
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
            inputState.CheckRaycastCamera();

#if UNITY_ANDROID || UNITY_IOS
            GameObject hitGameObject = null;
            var touches = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches;
            int numTouches = touches.Count;

            if (numTouches > 0)
            {
                // Only update pointer state if we've actually touched down, otherwise leave it as it was the last time we touched the screen
                // The hit game object and touches logic in the InputState should flag to other systems we've not hit anything
                Vector3 touchPosition = touches[0].screenPosition;
                ValueTuple<Vector3, GameObject> hitObject = inputState.CalculateHitObjectAndWorldPosition(touchPosition, touches[0].touchId, eventSystem, uiInputModule);
                Vector3 touchWorldPosition = hitObject.Item1;
                hitGameObject = hitObject.Item2;

                inputState.UpdatePointerPosition(touchPosition, touchWorldPosition);
            }
            
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
            ValueTuple<Vector3, GameObject> hitObject = inputState.CalculateHitObjectAndWorldPosition(mousePosition, 0, eventSystem, uiInputModule);
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

        // DO NOT DELETE, USED FOR PLATFORMS OTHER THAN IOS AND ANDROID
        private void CheckMouseButton(ButtonControl buttonControl, MouseButton mouseButton)
        {
            PointerState mouseButtonState = new PointerState()
            {
                wasFirstDownThisFrame = buttonControl.wasPressedThisFrame,
                isDown = buttonControl.isPressed,
                wasFirstUpThisFrame = buttonControl.wasReleasedThisFrame,
            };

            inputState.UpdateMouseButtonState(mouseButton, mouseButtonState);
        }

        #endregion

        #region Callbacks

        public void SetInputCamera(Camera camera)
        {
            inputState.RaycastCamera = camera;
        }

        #endregion
    }
}
