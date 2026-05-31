using Celeste.Events;
using Celeste.Parameters;
using System;
using Celeste.Tools;
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
                Bounds bounds = t.localBounds;
                Vector3 minWorldSpace = t.layoutGrid.LocalToWorld(bounds.min) - new Vector3(xPadding, yPadding, 0);
                Vector3 maxWorldSpace = t.layoutGrid.LocalToWorld(bounds.max) + new Vector3(xPadding, yPadding, 0);
                float mapWidth = Mathf.Abs(maxWorldSpace.x - minWorldSpace.x);
                float mapHeight = Mathf.Abs(maxWorldSpace.y - minWorldSpace.y);
                float sizeNeededForHeight = mapHeight / 2f;
                float cameraWidthNeeded = mapWidth / 2f;
                float sizeNeededForWidth = cameraWidthNeeded / cameraToZoom.aspect;
                
                return Mathf.Max(sizeNeededForHeight, sizeNeededForWidth);
            }
        }

        [SerializeField] private Camera cameraToZoom;
        [SerializeField] private TilemapReference tilemap;
        [SerializeField] private float xPadding;
        [SerializeField] private float yPadding;
        [SerializeField] private FloatReference minZoom;
        [SerializeField] private FloatReference maxZoom;
        [SerializeField] private FloatReference zoomSpeed;
        
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

#endregion
    }
}
