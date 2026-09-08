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
        [SerializeField] private RectTransform safeArea;

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
            TilemapRenderer renderer = tilemap.Value.GetComponent<TilemapRenderer>();
            Vector3 mapCenterWorld = renderer.bounds.center;

            // Get the normalized center of the safe area in Viewport space (0,0 bottom-left to 1,1 top-right)
            Vector3[] corners = new Vector3[4];
            safeArea.GetWorldCorners(corners);
    
            // Convert safe area center pixel coordinate to normalized Viewport space (0.0 - 1.0)
            Vector2 safeAreaCenterPixels = (corners[0] + corners[2]) * 0.5f;
            Vector2 safeAreaCenterViewport = new Vector2(
                safeAreaCenterPixels.x / Screen.currentResolution.width,
                safeAreaCenterPixels.y / Screen.currentResolution.height
            );

            // Calculate the viewport offset relative to the screen center (0.5, 0.5)
            Vector2 offsetFromCenter = safeAreaCenterViewport - new Vector2(0.5f, 0.5f);

            // Convert the viewport offset to world distance based on orthographic size
            float orthoSize = cameraToDrag.orthographicSize;
            float worldOffsetY = offsetFromCenter.y * (orthoSize * 2f);
            float worldOffsetX = offsetFromCenter.x * (orthoSize * 2f * cameraToDrag.aspect);

            // Position camera so tilemap center aligns with safe area center
            Vector3 targetCamPosition = mapCenterWorld - new Vector3(worldOffsetX, worldOffsetY, 0f);
            targetCamPosition.z = cameraToDrag.transform.position.z; // Preserve Z depth

            cameraToDrag.transform.position = targetCamPosition;
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
            /*Tilemap t = tilemap.Value;
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

            transform.position = new Vector3(clampedX, clampedY, cameraPosition.z);*/
        }

        #endregion
    }
}
