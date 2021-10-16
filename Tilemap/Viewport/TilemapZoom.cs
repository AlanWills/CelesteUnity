using Celeste.Events;
using Celeste.Parameters;
using System;
using UnityEngine;

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
                Bounds bounds = tilemap.Value.localBounds;
                float xSize = (bounds.size.x + xPadding / ppu) * Screen.height / (Screen.width * 2.0f);
                float ySize = (bounds.size.y + yPadding / ppu) * 0.5f;

                return Math.Max(xSize, ySize);
            }
        }

        [SerializeField]
        private TilemapValue tilemap;

        [SerializeField]
        private float ppu;

        [SerializeField]
        private float xPadding;

        [SerializeField]
        private float yPadding;

        [SerializeField]
        private FloatValue minZoom;

        [SerializeField]
        private FloatValue maxZoom;

        [SerializeField]
        private FloatValue zoomSpeed;
        
        private Camera cameraToZoom;

        #endregion

        #region Unity Methods

        private void Start()
        {
            cameraToZoom = GetComponent<Camera>();
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
            Debug.AssertFormat(touchEventArgs.touchCount == 2, "Expected 2 touches for ZoomUsingPinch, but got {0}", touchEventArgs.touchCount);
            if (Input.touchCount == 2)
            {
                // Store both touches.
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                ApplyZoom(deltaMagnitudeDiff * zoomSpeed.Value);
            }
        }

        private void ApplyZoom(float zoomAmount)
        {
            // Zoom out
            cameraToZoom.orthographicSize += zoomAmount;
            
            FitCamera();
        }

        private void FitCamera()
        {
            float fitSize = FitSize;
            cameraToZoom.orthographicSize = Mathf.Clamp(cameraToZoom.orthographicSize, fitSize / maxZoom.Value, fitSize / minZoom.Value);
        }

        public void FullZoomOut()
        {
            cameraToZoom.orthographicSize = FitSize;
        }

        #endregion
    }
}
