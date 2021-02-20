using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Debugging.Info
{
    [AddComponentMenu("Robbi/Debugging/FPS")]
    public class FPS : MonoBehaviour
    {
        #region Properties and Fields

        public Text text;

        private Coroutine measurementCoroutine;

        #endregion

        #region FPS Measurement

        private IEnumerator MeasureFPS()
        {
            while (true)
            {
                text.text = string.Format("{0}", Mathf.RoundToInt(1.0f / Time.unscaledDeltaTime));

                yield return new WaitForSecondsRealtime(1);
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
