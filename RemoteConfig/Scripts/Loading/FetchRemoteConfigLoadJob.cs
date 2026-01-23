using Celeste.Loading;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.RemoteConfig
{
    [CreateAssetMenu(fileName = nameof(FetchRemoteConfigLoadJob), order = CelesteMenuItemConstants.REMOTECONFIG_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.REMOTECONFIG_MENU_ITEM + "Loading/Fetch Remote Config")]
    public class FetchRemoteConfigLoadJob : LoadJob
    {
        #region Properties and Fields

        [SerializeField] private RemoteConfigRecord remoteConfigRecord;

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            setOutput("Fetching Data");

            UnityEngine.Debug.Assert(remoteConfigRecord != null, $"Remote Config Record was null in load job {name}!  Fetching remote config will be skipped...");
            if (remoteConfigRecord != null)
            {
                Task fetchTask = remoteConfigRecord.FetchData();
                setProgress(0.25f);

                while (!fetchTask.IsCompleted)
                {
                    yield return null;
                }

                setOutput(fetchTask.IsCompletedSuccessfully ? "Fetching Data Succeeded!" : "Fetching Data Failed!");
            }

            setProgress(1.0f);
        }
    }
}
