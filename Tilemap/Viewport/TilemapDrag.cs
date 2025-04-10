﻿using System;
using Celeste.Parameters;
using UnityEngine;
#if USE_NEW_INPUT_SYSTEM
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
#else
using Touch = UnityEngine.Touch;
#endif

namespace Celeste.Tilemaps
{
    [AddComponentMenu("Celeste/Tilemaps/Tilemap Drag")]
    [RequireComponent(typeof(Camera))]
    public class TilemapDrag : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private TilemapReference tilemap;
        [SerializeField] private FloatReference dragSpeed;

        private Camera cameraToDrag;
        private float timeSinceFingerDown = 0;
        private const float DRAG_THRESHOLD = 0.1f;
        private bool dragStarted = false;
        private Vector2 previousMouseDownPosition = new Vector2();

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (tilemap == null)
            {
                tilemap = ScriptableObject.CreateInstance<TilemapReference>();
            }

            if (dragSpeed == null)
            {
                dragSpeed = ScriptableObject.CreateInstance<FloatReference>();
                dragSpeed.IsConstant = true;
                dragSpeed.Value = 1f;
            }
        }

        private void Start()
        {
            cameraToDrag = GetComponent<Camera>();
        }

        #endregion

        #region Utility Functions

        public void StartDrag(Vector2 mousePosition)
        {
            dragStarted = true;
            previousMouseDownPosition = mousePosition;
        }

        public void DragUsingMouse(Vector2 mousePosition)
        {
            if (dragStarted)
            {
                Vector3 previousMouseDownWorldPosition = cameraToDrag.ScreenToWorldPoint(previousMouseDownPosition);
                Vector3 mouseWorldPosition = cameraToDrag.ScreenToWorldPoint(mousePosition);
                Vector2 mouseDelta = previousMouseDownWorldPosition - mouseWorldPosition; // We need to go in the opposite direction to the drag
                mouseDelta *= dragSpeed.Value;

                transform.position += new Vector3(mouseDelta.x, mouseDelta.y, 0);
                previousMouseDownPosition = mousePosition;

                ClampCamera();
            }
        }

        public void EndDrag()
        {
            dragStarted = false;
        }

        public void DragUsingTouch(Touch touch)
        {
#if USE_NEW_INPUT_SYSTEM
            switch (touch.phase)
            {
                case UnityEngine.InputSystem.TouchPhase.Began:
                    timeSinceFingerDown = 0;
                    break;

                case UnityEngine.InputSystem.TouchPhase.Stationary:
                    timeSinceFingerDown += Time.deltaTime;
                    break;

                case UnityEngine.InputSystem.TouchPhase.Moved:
                    timeSinceFingerDown += Time.deltaTime;

                    if (timeSinceFingerDown >= DRAG_THRESHOLD)
                    {
                        Vector2 touchPosition = touch.screenPosition;
                        Vector3 previousTouchDownWorldPosition = cameraToDrag.ScreenToWorldPoint(touchPosition - touch.delta);
                        Vector3 touchWorldPosition = cameraToDrag.ScreenToWorldPoint(touchPosition);
                        Vector2 dragAmount = previousTouchDownWorldPosition - touchWorldPosition; // We need to go in the opposite direction to the drag
                        float scrollModifier = dragSpeed.Value;
                        
                        transform.Translate(dragAmount.x * scrollModifier, dragAmount.y * scrollModifier, 0);
                        ClampCamera();
                    }
                    break;

                default:
                    timeSinceFingerDown = 0;
                    break;
            }
#endif
        }

        private void ClampCamera()
        {
            Bounds bounds = tilemap.Value.localBounds;
            Vector3 currentPosition = transform.position;
            currentPosition.x = Mathf.Clamp(currentPosition.x, bounds.min.x, bounds.max.x);
            currentPosition.y = Mathf.Clamp(currentPosition.y, bounds.min.y, bounds.max.y);
            transform.position = currentPosition;
        }

        #endregion
    }
}
