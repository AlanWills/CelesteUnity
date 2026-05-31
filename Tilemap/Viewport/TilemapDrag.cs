using System;
using Celeste.Parameters;
using UnityEngine;
using UnityEngine.Tilemaps;
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
        [SerializeField] private float xPadding;
        [SerializeField] private float yPadding;

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

        public void CentreCamera()
        {
            Tilemap t = tilemap.Value;
            Vector3 worldMin = t.transform.TransformPoint(t.localBounds.min); 
            Vector3 worldMax = t.transform.TransformPoint(t.localBounds.max);
            Vector3 currentPosition = transform.position;
            currentPosition.x = (worldMin.x + worldMax.x) / 2;
            currentPosition.y = (worldMin.y + worldMax.y) / 2;
            transform.position = currentPosition;
        }

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

        public void ClampCamera()
        {
            Tilemap t = tilemap.Value;
            Bounds bounds = t.localBounds;
            Vector3 worldSpaceMin = t.layoutGrid.LocalToWorld(bounds.min) - new Vector3(xPadding, yPadding, 0);
            Vector3 worldSpaceMax = t.layoutGrid.LocalToWorld(bounds.max) + new Vector3(xPadding, yPadding, 0);
            
            float mapMinX = Mathf.Min(worldSpaceMin.x, worldSpaceMax.x);
            float mapMaxX = Mathf.Max(worldSpaceMin.x, worldSpaceMax.x);
            float mapMinY = Mathf.Min(worldSpaceMin.y, worldSpaceMax.y);
            float mapMaxY = Mathf.Max(worldSpaceMin.y, worldSpaceMax.y);
            float camHalfHeight = cameraToDrag.orthographicSize;
            float camHalfWidth = camHalfHeight * cameraToDrag.aspect;
            
            float minX = mapMinX + camHalfWidth;
            float maxX = mapMaxX - camHalfWidth;
            float minY = mapMinY + camHalfHeight;
            float maxY = mapMaxY - camHalfHeight;

            Vector3 cameraPosition = transform.position;
            
            if (maxX < minX)
            {
                minX = maxX = (mapMinX + mapMaxX) / 2f;
            }
            
            if (maxY < minY)
            {
                minY = maxY = (mapMinY + mapMaxY) / 2f;
            }

            float clampedX = Mathf.Clamp(cameraPosition.x, minX, maxX);
            float clampedY = Mathf.Clamp(cameraPosition.y, minY, maxY);

            transform.position = new Vector3(clampedX, clampedY, cameraPosition.z);
        }

        #endregion
    }
}
