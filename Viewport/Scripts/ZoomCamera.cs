using Celeste.Events;
using Celeste.Parameters;
using System.Collections;
using UnityEngine;

namespace Celeste.Viewport
{
    [AddComponentMenu("Celeste/Viewport/Zoom Camera")]
    public class ZoomCamera : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Camera cameraToZoom;
        [SerializeField] private FloatReference minZoom;
        [SerializeField] private FloatReference maxZoom;
        [SerializeField] private FloatReference zoomSpeed;
        [SerializeField] private float animateSpeed = 0.1f;

        private Coroutine zoomCoroutine;

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

            if (zoomCoroutine != null)
            {
                StopCoroutine(zoomCoroutine);
            }

            StartCoroutine(ZoomImpl(scrollAmount));
        }

        private IEnumerator ZoomImpl(float scrollAmount)
        {
            scrollAmount *= zoomSpeed.Value;

            float currentAnimationTime = 0;

            if (cameraToZoom.orthographic)
            {
                float startingSize = cameraToZoom.orthographicSize;
                float finishingSize = Mathf.Clamp(startingSize - scrollAmount, minZoom.Value, maxZoom.Value);
                float animationTime = Mathf.Abs(finishingSize - startingSize) / animateSpeed;

                while (currentAnimationTime < animationTime)
                {
                    currentAnimationTime += Time.deltaTime;
                    
                    float lerpAmount = currentAnimationTime / animationTime;
                    cameraToZoom.orthographicSize = Mathf.Lerp(startingSize, finishingSize, lerpAmount);

                    yield return null;
                }
            }
            else
            {
                Vector3 position = transform.localPosition;
                float startingZ = position.z;
                float finishingZ = position.z + scrollAmount;
                float animationTime = Mathf.Abs(scrollAmount) / animateSpeed;

                while (currentAnimationTime < animationTime)
                {
                    currentAnimationTime += Time.deltaTime;
                    
                    float lerpAmount = currentAnimationTime / animationTime;
                    position = transform.localPosition;
                    position.z = Mathf.Lerp(startingZ, finishingZ, lerpAmount);
                    transform.localPosition = position;

                    yield return null;
                }
            }
        }

        public void ZoomUsingPinch(MultiTouchEventArgs touchEventArgs)
        {
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

                cameraToZoom.orthographicSize = Mathf.Clamp(cameraToZoom.orthographicSize + deltaMagnitudeDiff * zoomSpeed.Value, minZoom.Value, maxZoom.Value);
            }
        }

        #endregion
    }
}
