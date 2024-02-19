using Celeste.Events;
using System;
using UnityEngine;

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

    [CreateAssetMenu(fileName = nameof(InputState), menuName = "Celeste/Input/Input State")]
    public class InputState : ScriptableObject
    {
        #region Properties and Fields

        public Vector2 PointerPosition { get; set; }
        public Vector3 PointerWorldPosition { get; set; }

        public GameObject HitGameObject { get; private set; }
        public PointerState PointerState { get; private set; }
        public PointerState LeftMouseButton { get; private set; }
        public PointerState MiddleMouseButton { get; private set; }
        public PointerState RightMouseButton { get; private set; }

        [Header("Events")]
        [SerializeField] private GameObjectEvent pointerEnteredGameObjectEvent;
        [SerializeField] private GameObjectEvent pointerExitedGameObjectEvent;

        #endregion

        public void PointerOverObject(GameObject hitGameObject, int numTouches)
        {
            PointerOverObject(hitGameObject, numTouches == 1);
        }

        public void PointerOverObject(GameObject newHitGameObject, bool isDownThisFrame)
        {
            GameObject oldHitGameObject = HitGameObject;
            PointerState oldPointerState = PointerState;
            PointerState newPointerState = new PointerState()
            {
                wasFirstDownThisFrame = !oldPointerState.isDown && isDownThisFrame,
                isDown = isDownThisFrame,
                wasFirstUpThisFrame = oldPointerState.isDown && !isDownThisFrame
            };

            // Update the state first, but don't fire events
            PointerState = newPointerState;
            HitGameObject = newHitGameObject;

            // Now take care of figuring out which events we need to fire

            IInputHandler currentInputHandler = oldHitGameObject != null ? oldHitGameObject.GetComponent<IInputHandler>() : null;
            IInputHandler newInputHandler = newHitGameObject != null ? newHitGameObject.GetComponent<IInputHandler>() : null;

            if (oldHitGameObject != newHitGameObject)
            {
                if (oldHitGameObject != null)
                {
                    currentInputHandler?.OnPointerExit(this);
                    pointerExitedGameObjectEvent.Invoke(oldHitGameObject);
                }

                if (newHitGameObject != null)
                {
                    newInputHandler?.OnPointerEnter(this);
                    pointerEnteredGameObjectEvent.Invoke(newHitGameObject);
                }
            }
            else if (oldHitGameObject != null)
            {
                currentInputHandler?.OnPointerOver(this);
            }

            // If the pointer is over something, we should tell it if we've pressed it or not
            if (newHitGameObject != null)
            {
                if (newPointerState.wasFirstDownThisFrame)
                {
                    newInputHandler?.OnPointerFirstDown(this);
                }

                if (newPointerState.isDown)
                {
                    newInputHandler?.OnPointerDown(this);
                }

                if (newPointerState.wasFirstUpThisFrame)
                {
                    newInputHandler?.OnPointerFirstUp(this);
                }
            }
        }

        public void SetMouseButton(MouseButton mouseButton, PointerState mouseButtonState)
        {
            switch (mouseButton)
            {
                case MouseButton.Left:
                    LeftMouseButton = mouseButtonState;
                    break;

                case MouseButton.Middle:
                    MiddleMouseButton = mouseButtonState;
                    break;

                case MouseButton.Right:
                    RightMouseButton = mouseButtonState;
                    break;

                default:
                    break;
            }
        }

        public void ReleaseInput()
        {
            LeftMouseButton = new PointerState()
            {
                wasFirstDownThisFrame = false,
                isDown = false,
                wasFirstUpThisFrame = true,
            };

            MiddleMouseButton = new PointerState()
            {
                wasFirstDownThisFrame = false,
                isDown = false,
                wasFirstUpThisFrame = true,
            };

            RightMouseButton = new PointerState()
            {
                wasFirstDownThisFrame = false,
                isDown = false,
                wasFirstUpThisFrame = true,
            };

            PointerState = LeftMouseButton;
        }
    }
}
