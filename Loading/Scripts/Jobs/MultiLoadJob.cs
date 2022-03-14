using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Loading
{
    [CreateAssetMenu(fileName = nameof(MultiLoadJob), menuName = "Celeste/Loading/Load Jobs/Multi")]
    public class MultiLoadJob : LoadJob
    {
        #region Builder

        public class Builder
        {
            private List<LoadJob> loadJobs = new List<LoadJob>();

            public Builder WithLoadJob(LoadJob loadJob)
            {
                loadJobs.Add(loadJob);
                return this;
            }

            public MultiLoadJob Build()
            {
                MultiLoadJob multiLoadJob = CreateInstance<MultiLoadJob>();
                multiLoadJob.name = nameof(MultiLoadJob);
                multiLoadJob.loadJobs = loadJobs.ToArray();

                return multiLoadJob;
            }
        }

        #endregion

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