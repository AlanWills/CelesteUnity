using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Loading
{
    [CreateAssetMenu(fileName = nameof(MultiLoadJob), menuName = "Celeste/Loading/Load Jobs/Multi")]
    public class MultiLoadJob : LoadJob
    {
        #region Properties and Fields

        [SerializeField] private LoadJob[] loadJobs;

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            Debug.Assert(loadJobs != null, $"No load jobs set in {nameof(MultiLoadJob)} '{name}'.");
            for (int i = 0, n = loadJobs != null ? loadJobs.Length : 0; i < n; ++i)
            {
                yield return loadJobs[i].Execute(setProgress, setOutput);
            }
        }
    }
}