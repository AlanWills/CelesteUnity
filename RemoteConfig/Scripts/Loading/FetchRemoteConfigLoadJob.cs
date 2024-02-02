using Celeste.Loading;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.RemoteConfig
{
    [CreateAssetMenu(fileName = nameof(FetchRemoteConfigLoadJob), menuName = "Celeste/Remote Config/Loading/Fetch Remote Config")]
    public class FetchRemoteConfigLoadJob : LoadJob
    {
        #region Properties and Fields

        [SerializeField] private RemoteConfigRecord remoteConfigRecord;

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            setOutput("Fetching Data");

            Task fetchTask = remoteConfigRecord.FetchData();
            setProgress(0.25f);

            while (!fetchTask.IsCompleted)
            {
                yield return null;
            }

            if (fetchTask.IsCompletedSuccessfully)
            {
                setOutput("Fetching Data Succeeded!");
            }
            else
            {
                setOutput("Fetching Data Failed!");
            }

            setProgress(1.0f);
        }
    }
}
