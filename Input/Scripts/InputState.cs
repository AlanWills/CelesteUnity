﻿using System;
using UnityEngine;

namespace Celeste.Input
{
    [Serializable]
    public struct MouseButtonState
    {
        public bool wasPressedThisFrame;
        public bool isPressed;
        public bool wasReleasedThisFrame;
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

        public Vector2 MousePosition { get; set; }
        public Vector3 MouseWorldPosition { get; set; }

        [NonSerialized] private GameObject hitGameObject;
        public GameObject HitGameObject
        {
            get => hitGameObject;
            set
            {
                if (hitGameObject != value)
                {
                    if (hitGameObject != null)
                    {
                        hitGameObject.SendMessage(onMouseExitMessage, SendMessageOptions.DontRequireReceiver);
                    }

                    hitGameObject = value;

                    if (hitGameObject != null)
                    {
                        hitGameObject.SendMessage(onMouseEnterMessage, MousePosition, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }

        public MouseButtonState LeftMouseButton { get; set; }
        public MouseButtonState MiddleMouseButton { get; set; }
        public MouseButtonState RightMouseButton { get; set; }

        [SerializeField] private string onMouseEnterMessage = "OnMouseEnter";
        [SerializeField] private string onMouseExitMessage = "OnMouseExit";

        #endregion

        public MouseButtonState GetMouseButton(MouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButton.Left:
                    return LeftMouseButton;

                case MouseButton.Middle:
                    return MiddleMouseButton;

                case MouseButton.Right:
                    return RightMouseButton;

                default:
                    return new MouseButtonState();
            }
        }

        public void SetMouseButton(MouseButton mouseButton, MouseButtonState mouseButtonState)
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
    }
}