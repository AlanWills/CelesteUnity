using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Viewport
{
    [AddComponentMenu("Celeste/Viewport/Zoom Camera")]
    public class ZoomCamera : MonoBehaviour
    {
        #region Properties and Fields

        public Camera cameraToZoom;
        public FloatReference minZoom;
        public FloatReference maxZoom;
        public FloatReference zoomSpeed;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (cameraToZoom == null)
            {
                cameraToZoom = GetComponent<Camera>();
            }

            if (minZoom == null)
            {
                minZoom = ScriptableObject.CreateInstance<FloatReference>();
                minZoom.IsConstant = true;
            }

            if (maxZoom == null)
            {
                maxZoom = ScriptableObject.CreateInstance<FloatReference>();
                maxZoom.IsConstant = true;
            }

            if (zoomSpeed == null)
            {
                zoomSpeed = ScriptableObject.CreateInstance<FloatReference>();
                zoomSpeed.IsConstant = true;
            }
        }

        #endregion

        #region Zoom Utility Methods

        public void ZoomUsingScroll(float scrollAmount)
        {
            if (scrollAmount == 0)
            {
                return;
            }

            if (cameraToZoom.orthographic)
            {
                cameraToZoom.orthographicSize = Mathf.Clamp(cameraToZoom.orthographicSize - scrollAmount, minZoom.Value, maxZoom.Value);
            }
            else
            {
                Vector3 position = transform.localPosition;
                position.z = Mathf.Clamp(position.z + scrollAmount, minZoom.Value, maxZoom.Value);
                transform.localPosition = position;
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

                cameraToZoom.orthographicSize = Mathf.Clamp(cameraToZoom.orthographicSize + deltaMagnitudeDiff * zoomSpeed.Value, minZoom.Value, maxZoom.Value);
            }
        }

        #endregion
    }
}
