using Celeste.Events;
using Celeste.Parameters;
using System;
using Celeste.Tools;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Celeste.Tilemaps
{
    [AddComponentMenu("Celeste/Tilemaps/Tilemap Zoom")]
    [RequireComponent(typeof(Camera))]
    public class TilemapZoom : MonoBehaviour
    {
        #region Properties and Fields

        public float FitSize
        {
            get
            {
                Tilemap t = tilemap.Value;
                Bounds bounds = t.GetComponent<TilemapRenderer>().bounds;
                
                // Scale safeArea.rect by lossyScale in case Canvas Scaler is active
                Vector3[] corners = new Vector3[4];
                safeArea.GetWorldCorners(corners);
    
                float safeWidthInPixels = Vector3.Distance(corners[0], corners[3]);
                float safeHeightInPixels = Vector3.Distance(corners[0], corners[1]);

                // Screen dimensions
                float screenWidth = Screen.currentResolution.width;
                float screenHeight = Screen.currentResolution.height;

                // 4. Calculate the fraction of the screen occupied by the safe area (0.0 to 1.0)
                float safeAreaWidthRatio = safeWidthInPixels / screenWidth;
                float safeAreaHeightRatio = safeHeightInPixels / screenHeight;

                // 5. Calculate world size required to fit inside the Safe Area
                // If safe area is half the screen height, the camera needs twice the vertical height to fit the map
                float effectiveWorldWidthNeeded = bounds.size.x / safeAreaWidthRatio;
                float effectiveWorldHeightNeeded = bounds.size.y / safeAreaHeightRatio;

                // 6. Calculate orthographic sizes required for both axes
                float sizeNeededForHeight = effectiveWorldWidthNeeded / 2f / cameraToZoom.aspect; // Width constrained
                float sizeNeededForVerticalHeight = effectiveWorldHeightNeeded / 2f;         // Height constrained

                // Return the larger size to ensure the map fits both horizontally and vertically inside the safe area
                return Mathf.Max(sizeNeededForVerticalHeight, sizeNeededForHeight);
            }
        }

        [SerializeField] private Camera cameraToZoom;
        [SerializeField] private TilemapReference tilemap;
        [SerializeField] private RectTransform safeArea;
        [SerializeField] private FloatReference minZoom;
        [SerializeField] private FloatReference maxZoom;
        [SerializeField] private FloatReference zoomSpeed;
        [SerializeField] private FloatReference zoomIncrement;
        
        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref cameraToZoom);
            
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
        }

        #endregion

        #region Zoom Utility Methods

        public void ZoomPercentage(float percentage)
        {
            ApplyZoom(FitSize * (-percentage / 100.0f));
        }

        public void ZoomUsingScroll(float scrollAmount)
        {
            if (scrollAmount != 0)
            {
                ApplyZoom(-scrollAmount * zoomSpeed.Value);
            }
        }

        public void ZoomUsingPinch(MultiTouchEventArgs touchEventArgs)
        {
#if USE_NEW_INPUT_SYSTEM
            Debug.AssertFormat(touchEventArgs.touchCount == 2, "Expected 2 touches for ZoomUsingPinch, but got {0}", touchEventArgs.touchCount);
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

                ApplyZoom(deltaMagnitudeDiff * zoomSpeed.Value);
            }
#endif
        }

        private void ApplyZoom(float zoomAmount)
        {
            // Zoom out
            cameraToZoom.orthographicSize += zoomAmount;
            
            ClampCamera();
        }

        private void ClampCamera()
        {
            float fitSize = FitSize;
            
            // To handle the situations where fitSize goes below the bounds of these two values
            // We must ensure we have sufficient zoom to fit the tilemap otherwise things will just look odd
            float minSize = Mathf.Min(fitSize, minZoom.Value);
            float maxSize = Mathf.Min(fitSize, maxZoom.Value);
            cameraToZoom.orthographicSize = Mathf.Clamp(cameraToZoom.orthographicSize, minSize, maxSize);
        }

        public void FitCamera()
        {
            cameraToZoom.orthographicSize = FitSize;
            
            ClampCamera();
        }

        public void ZoomOutIncrement()
        {
            ApplyZoom(zoomIncrement.Value);
        }
        
        public void ZoomInIncrement()
        {
            ApplyZoom(zoomIncrement.Value * -1);
        }

#endregion
    }
}
