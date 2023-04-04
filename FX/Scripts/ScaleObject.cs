using Celeste.Events;
using Celeste.Parameters;
using Celeste.Tools;
using System.Collections;
using UnityEngine;

namespace Celeste.FX
{
    [AddComponentMenu("Celeste/FX/Scale Object")]
    public class ScaleObject : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Transform transformToZoom;
        [SerializeField] private FloatReference minZoom;
        [SerializeField] private FloatReference maxZoom;
        [SerializeField] private FloatReference zoomSpeed;
        [SerializeField] private float animateSpeed = 0.1f;

        private Coroutine zoomCoroutine;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref transformToZoom);

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

            float startingScale = transformToZoom.localScale.x;
            float finishingScale = Mathf.Clamp(startingScale - scrollAmount, minZoom.Value, maxZoom.Value);
            float animationTime = Mathf.Abs(finishingScale - startingScale) / animateSpeed;

            while (currentAnimationTime < animationTime)
            {
                currentAnimationTime += Time.deltaTime;

                float lerpAmount = currentAnimationTime / animationTime;
                float newScale = Mathf.Lerp(startingScale, finishingScale, lerpAmount);
                transformToZoom.localScale = new Vector3(newScale, newScale, 1);

                yield return null;
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
                float newScale = Mathf.Clamp(transform.localScale[0] + deltaMagnitudeDiff * zoomSpeed.Value, minZoom.Value, maxZoom.Value);

                transformToZoom.localScale = new Vector3(newScale, newScale, 1);
            }
        }

        #endregion
    }
}
