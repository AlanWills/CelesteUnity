using Celeste.Tools;
using System;
using System.Collections;
using UnityEngine;

namespace Celeste.FX
{
    [Serializable]
    public struct LerpParameters
    {
        public float lerpInDelay;
        public float lerpInTime;
        public float lerpOutDelay;
        public float lerpOutTime;
        public float lerpStartAlpha;
        public float lerpEndAlpha;
    }

    [AddComponentMenu("Celeste/FX/Canvas Group Alpha Lerper")]
    public class CanvasGroupAlphaLerper : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private bool hideOnAwake = true;
        [SerializeField] private CanvasGroup canvasGroup;

        [NonSerialized] private Coroutine lerpCoroutine;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref canvasGroup);
        }

        private void Awake()
        {
            if (hideOnAwake)
            {
                Hide();
            }
        }

        #endregion

        public void Hide()
        {
            TryStopCoroutine();
            canvasGroup.alpha = 0;
        }

        public void Lerp(LerpParameters parameters)
        {
            TryStopCoroutine();
            lerpCoroutine = StartCoroutine(LerpImpl(parameters));
        }

        private void TryStopCoroutine()
        {
            if (lerpCoroutine != null)
            {
                StopCoroutine(lerpCoroutine);
                lerpCoroutine = null;
            }
        }

        private IEnumerator LerpImpl(LerpParameters parameters)
        {
            canvasGroup.alpha = parameters.lerpStartAlpha;

            if (parameters.lerpInDelay > 0)
            {
                yield return new WaitForSeconds(parameters.lerpInDelay);
            }

            float time = 0;

            while (time < parameters.lerpInTime)
            {
                canvasGroup.alpha = Mathf.Lerp(parameters.lerpStartAlpha, parameters.lerpEndAlpha, time / parameters.lerpInTime);
                time += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = parameters.lerpEndAlpha;
            
            yield return null;

            if (parameters.lerpOutDelay > 0)
            {
                yield return new WaitForSeconds(parameters.lerpOutDelay);
            }

            time = 0;

            while (time < parameters.lerpOutTime)
            {
                canvasGroup.alpha = Mathf.Lerp(parameters.lerpEndAlpha, parameters.lerpStartAlpha, time / parameters.lerpOutTime);
                time += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = parameters.lerpStartAlpha;
        }
    }
}
