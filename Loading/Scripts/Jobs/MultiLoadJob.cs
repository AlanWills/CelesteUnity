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
                if (loadJob != null)
                {
                    loadJobs.Add(loadJob);
                }

                return this;
            }

            public MultiLoadJob Build()
            {
                MultiLoadJob multiLoadJob = CreateInstance<MultiLoadJob>();
                multiLoadJob.name = nameof(MultiLoadJob);
                multiLoadJob.loadJobs.AddRange(loadJobs);

                return multiLoadJob;
            }
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private List<LoadJob> loadJobs = new List<LoadJob>();

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            Debug.Assert(loadJobs != null, $"No load jobs set in {nameof(MultiLoadJob)} '{name}'.");
            for (int i = 0, n = loadJobs.Count; i < n; ++i)
            {
                yield return loadJobs[i].Execute(setProgress, setOutput);
            }
        }
    }
}