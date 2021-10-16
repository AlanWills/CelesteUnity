using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;

namespace Celeste.Debug
{
    [AddComponentMenu("Celeste/Debug/Memory Info")]
    public class MemoryInfo : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField]
        private TextMeshProUGUI totalReservedSizeText;

        [SerializeField]
        private TextMeshProUGUI heapSizeText;

        private Coroutine statsCoroutine;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            statsCoroutine = StartCoroutine(UpdateStats());
        }

        private void OnDisable()
        {
            if (statsCoroutine != null)
            {
                StopCoroutine(statsCoroutine);
            }
        }

        #endregion

        private IEnumerator UpdateStats()
        {
            while (true)
            {
                totalReservedSizeText.text = string.Format("Reserved: {0}MB", Profiler.GetTotalReservedMemoryLong() / (1024 * 1024));
                heapSizeText.text = string.Format("Heap: {0}MB", Profiler.GetMonoHeapSizeLong() / (1024 * 1024));

                yield return new WaitForSeconds(1);
            }
        }
    }
}
