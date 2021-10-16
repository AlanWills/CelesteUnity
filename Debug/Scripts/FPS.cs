using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Debug.Info
{
    [AddComponentMenu("Celeste/Debug/FPS")]
    public class FPS : MonoBehaviour
    {
        #region Properties and Fields

        public TextMeshProUGUI text;

        private Coroutine measurementCoroutine;
        private WaitForSecondsRealtime waitForSeconds = new WaitForSecondsRealtime(1);

        #endregion

        #region FPS Measurement

        private IEnumerator MeasureFPS()
        {
            while (true)
            {
                text.text = string.Format("{0}", Mathf.RoundToInt(1.0f / Time.unscaledDeltaTime));

                waitForSeconds.Reset();
                yield return waitForSeconds;
            }
        }

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if (UnityEngine.Application.isPlaying)
            {
                measurementCoroutine = StartCoroutine(MeasureFPS());
            }
        }

        private void OnDisable()
        {
            if (UnityEngine.Application.isPlaying && measurementCoroutine != null)
            {
                StopCoroutine(measurementCoroutine);
            }
        }

        #endregion
    }
}
