using Celeste.Events;
using Celeste.Parameters;
using Celeste.Tools;
using UnityEngine;
using UnityEngine.Tilemaps;
#if USE_NEW_INPUT_SYSTEM
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
#else
using Touch = UnityEngine.Touch;
#endif

namespace Celeste.Tilemaps
{
    [AddComponentMenu("Celeste/Tilemaps/Tilemap Camera Controller")]
    [RequireComponent(typeof(Camera))]
    public class TilemapCameraController : MonoBehaviour
    {
        #region Properties and Fields

        private Bounds TilemapWorldBounds
        {
            get
            {
                Tilemap t = tilemap.Value;
                Bounds bounds = t.localBounds;
                Vector3 minWorldSpace = t.layoutGrid.LocalToWorld(bounds.min);
                Vector3 maxWorldSpace = t.layoutGrid.LocalToWorld(bounds.max);
                return new Bounds((maxWorldSpace + minWorldSpace) * 0.5f, maxWorldSpace - minWorldSpace);
            }
        }

        private float FitSize
        {
            get
            {
                GetSafeAreaRatios(out float safeAreaWidthRatio, out float safeAreaHeightRatio, out Vector2 _);
                
                Bounds bounds = TilemapWorldBounds;
                float effectiveWorldWidthNeeded = bounds.size.x / safeAreaWidthRatio;
                float effectiveWorldHeightNeeded = bounds.size.y / safeAreaHeightRatio;
                float sizeNeededForHeight = effectiveWorldWidthNeeded / (2f * cameraToControl.aspect);
                float sizeNeededForVerticalHeight = effectiveWorldHeightNeeded / 2f;

                return Mathf.Max(sizeNeededForVerticalHeight, sizeNeededForHeight);
            }
        }

        [SerializeField] private Camera cameraToControl;
        [SerializeField] private TilemapReference tilemap;
        [SerializeField] private RectTransform safeAreaRootCanvas;
        [SerializeField] private RectTransform safeArea;
        [SerializeField] private FloatReference minZoom;
        [SerializeField] private FloatReference maxZoom;
        [SerializeField] private FloatReference zoomSpeed;
        [SerializeField] private FloatReference zoomIncrement;
        [SerializeField] private FloatReference dragSpeed;

        private float timeSinceFingerDown = 0;
        private bool dragStarted = false;
        private Vector2 previousMouseDownPosition = new Vector2();
        private readonly Vector3[] safeAreaCorners = new Vector3[4];
        private readonly Vector3[] canvasCorners = new Vector3[4];

        private const float DRAG_THRESHOLD = 0.1f;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref cameraToControl);

            if (tilemap == null)
            {
                tilemap = ScriptableObject.CreateInstance<TilemapReference>();
            }

            if (minZoom == null)
            {
                minZoom = ScriptableObject.CreateInstance<FloatReference>();
                minZoom.IsConstant = true;
                minZoom.Value = 1f;
            }

            if (maxZoom == null)
            {
                maxZoom = ScriptableObject.CreateInstance<FloatReference>();
                maxZoom.IsConstant = true;
                maxZoom.Value = 1f;
            }

            if (zoomSpeed == null)
            {
                zoomSpeed = ScriptableObject.CreateInstance<FloatReference>();
                zoomSpeed.IsConstant = true;
                zoomSpeed.Value = 1f;
            }

            if (zoomIncrement == null)
            {
                zoomIncrement = ScriptableObject.CreateInstance<FloatReference>();
                zoomIncrement.IsConstant = true;
                zoomIncrement.Value = 1f;
            }

            if (dragSpeed == null)
            {
                dragSpeed = ScriptableObject.CreateInstance<FloatReference>();
                dragSpeed.IsConstant = true;
                dragSpeed.Value = 1f;
            }
        }

        #endregion

        #region Zoom Utility Methods

        public void ZoomPercentage(float percentage)
        {
            IncrementZoom(FitSize * (-percentage / 100.0f));
        }

        public void ZoomUsingScroll(float scrollAmount)
        {
            if (scrollAmount != 0)
            {
                IncrementZoom(-scrollAmount * zoomSpeed.Value);
            }
        }

        public void ZoomUsingPinch(MultiTouchEventArgs touchEventArgs)
        {
#if USE_NEW_INPUT_SYSTEM
            Debug.AssertFormat(touchEventArgs.touchCount == 2, "Expected 2 touches for ZoomUsingPinch, but got {0}",
                touchEventArgs.touchCount);
            if (touchEventArgs.touchCount == 2)
            {
                // Store both touches.
                var touchZero = touchEventArgs.touches[0];
                var touchOne = touchEventArgs.touches[1];

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.screenPosition - touchZero.delta;
                Vector2 touchOnePrevPos = touchOne.screenPosition - touchOne.delta;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.screenPosition - touchOne.screenPosition).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                IncrementZoom(deltaMagnitudeDiff * zoomSpeed.Value);
            }
#endif
        }

        private void IncrementZoom(float increment)
        {
            SetZoom(cameraToControl.orthographicSize + increment);
        }

        private void SetZoom(float zoomAmount)
        {
            // Zoom out
            cameraToControl.orthographicSize = zoomAmount;

            ClampCameraZoom();
            ClampCameraPosition();
        }

        public void FitCamera()
        {
            SetZoom(FitSize);
            CentreCamera();
        }

        public void ZoomOutIncrement()
        {
            IncrementZoom(zoomIncrement.Value);
        }

        public void ZoomInIncrement()
        {
            IncrementZoom(zoomIncrement.Value * -1);
        }

        private void ClampCameraZoom()
        {
            float fitSize = FitSize;

            // To handle the situations where fitSize goes below the bounds of these two values
            // We must ensure we have sufficient zoom to fit the tilemap otherwise things will just look odd
            float minSize = Mathf.Min(fitSize, minZoom.Value);
            float maxSize = maxZoom.Value;
            cameraToControl.orthographicSize = Mathf.Clamp(cameraToControl.orthographicSize, minSize, maxSize);
        }

        #endregion

        #region Drag Utility Functions

        public void CentreCamera()
        {
            GetSafeAreaRatios(out float _, out float _, out Vector2 safeAreaOffset);

            // Convert the viewport offset to world distance based on orthographic size
            float orthoSize = cameraToControl.orthographicSize;
            float worldOffsetY = safeAreaOffset.y * orthoSize * 2f;
            float worldOffsetX = safeAreaOffset.x * orthoSize * 2f * cameraToControl.aspect;

            // Position camera so tilemap center aligns with safe area center
            Bounds tilemapWorldBounds = TilemapWorldBounds;
            Vector3 targetCamPosition = tilemapWorldBounds.center - new Vector3(worldOffsetX, worldOffsetY, 0f);
            targetCamPosition.z = cameraToControl.transform.position.z; // Preserve Z depth

            cameraToControl.transform.position = targetCamPosition;
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
                Vector3 previousMouseDownWorldPosition = cameraToControl.ScreenToWorldPoint(previousMouseDownPosition);
                Vector3 mouseWorldPosition = cameraToControl.ScreenToWorldPoint(mousePosition);
                Vector2 mouseDelta =
                    previousMouseDownWorldPosition -
                    mouseWorldPosition; // We need to go in the opposite direction to the drag
                mouseDelta *= dragSpeed.Value;

                transform.position += new Vector3(mouseDelta.x, mouseDelta.y, 0);
                previousMouseDownPosition = mousePosition;

                ClampCameraPosition();
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
                        Vector3 previousTouchDownWorldPosition =
                            cameraToControl.ScreenToWorldPoint(touchPosition - touch.delta);
                        Vector3 touchWorldPosition = cameraToControl.ScreenToWorldPoint(touchPosition);
                        Vector2 dragAmount =
                            previousTouchDownWorldPosition -
                            touchWorldPosition; // We need to go in the opposite direction to the drag
                        float scrollModifier = dragSpeed.Value;

                        transform.Translate(dragAmount.x * scrollModifier, dragAmount.y * scrollModifier, 0);
                        ClampCameraPosition();
                    }

                    break;

                default:
                    timeSinceFingerDown = 0;
                    break;
            }
#endif
        }

        private void ClampCameraPosition()
        {
            GetSafeAreaRatios(out float safeWidthRatio, out float safeHeightRatio, out Vector2 centerOffsetViewport);
            
            Bounds mapBounds = TilemapWorldBounds;
            Vector3 mapCenter = mapBounds.center;
            Vector3 mapExtents = mapBounds.extents; // Half-width (x) and Half-height (y)

            float currentOrthoSize = cameraToControl.orthographicSize;
            float safeAreaHalfHeightWorld = currentOrthoSize * safeHeightRatio;
            float safeAreaHalfWidthWorld = currentOrthoSize * cameraToControl.aspect * safeWidthRatio;
            
            Vector3 cameraPosition = cameraToControl.transform.position;

            // Orthographic size represents distance from the center to the top of the screen, so we need to multiply by 2 here
            // to represent the full visible vertical distance
            float worldOffsetX = centerOffsetViewport.x * currentOrthoSize * 2f * cameraToControl.aspect;
            float worldOffsetY = centerOffsetViewport.y * currentOrthoSize * 2f;
            
            float clampedX = ClampAxis(
                cameraPosition.x, 
                mapCenter.x, 
                mapExtents.x, 
                safeAreaHalfWidthWorld,
                worldOffsetX);
            float clampedY = ClampAxis(
                cameraPosition.y, 
                mapCenter.y, 
                mapExtents.y, 
                safeAreaHalfHeightWorld,
                worldOffsetY);

            transform.position = new Vector3(clampedX, clampedY, cameraPosition.z);
        }

        #endregion
        
        private void GetSafeAreaRatios(out float widthRatio, out float heightRatio, out Vector2 centerOffsetViewport)
        {
            safeAreaRootCanvas.GetWorldCorners(canvasCorners);
            safeArea.GetWorldCorners(safeAreaCorners);
            
            float canvasWidth = Mathf.Abs(canvasCorners[2].x - canvasCorners[0].x);
            float canvasHeight = Mathf.Abs(canvasCorners[2].y - canvasCorners[0].y);

            float safeWidth = Mathf.Abs(safeAreaCorners[2].x - safeAreaCorners[0].x);
            float safeHeight = Mathf.Abs(safeAreaCorners[2].y - safeAreaCorners[0].y);
            
            widthRatio = Mathf.Clamp01(safeWidth / canvasWidth);
            heightRatio = Mathf.Clamp01(safeHeight / canvasHeight);
            
            Vector3 canvasCenter = (canvasCorners[0] + canvasCorners[2]) * 0.5f;
            Vector3 safeCenter = (safeAreaCorners[0] + safeAreaCorners[2]) * 0.5f;

            centerOffsetViewport = new Vector2(
                (safeCenter.x - canvasCenter.x) / canvasWidth,
                (safeCenter.y - canvasCenter.y) / canvasHeight
            );
        }

        private static float ClampAxis(
            float currentCamPos, 
            float mapCenter, 
            float mapExtent, 
            float safeAreaHalfDimension,
            float offset)
        {
            // If map is larger than the safe area viewport (zoomed in): allow panning within bounds
            if (mapExtent > safeAreaHalfDimension)
            {
                float minCamPos = (mapCenter - mapExtent + safeAreaHalfDimension) - offset;
                float maxCamPos = (mapCenter + mapExtent - safeAreaHalfDimension) - offset;
                return Mathf.Clamp(currentCamPos, minCamPos, maxCamPos);
            }

            // If map is smaller than or equal to safe area (zoomed out): lock camera center to map center
            return mapCenter - offset;
        }
    }
}