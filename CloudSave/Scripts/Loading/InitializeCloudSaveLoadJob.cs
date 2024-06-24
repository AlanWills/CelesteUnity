using Celeste.Loading;
using Celeste.Persistence.Snapshots;
using System;
using System.Collections;
using UnityEngine;

namespace Celeste.CloudSave
{
    [CreateAssetMenu(
        fileName = nameof(InitializeCloudSaveLoadJob), 
        menuName = CelesteMenuItemConstants.CLOUDSAVE_MENU_ITEM + "Loading/Initialize Cloud Save",
        order = CelesteMenuItemConstants.CLOUDSAVE_MENU_ITEM_PRIORITY)]
    public class InitializeCloudSaveLoadJob : LoadJob
    {
        #region Properties and Fields

        [SerializeField] private CloudSaveRecord cloudSaveRecord;

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            setOutput("Activating Cloud Save");

            yield return null;

            cloudSaveRecord.Activate();
            setProgress(0.25f);
            setOutput("Cloud Save Activated");

            yield return cloudSaveRecord.AuthenticateAsync();

            if (cloudSaveRecord.IsAuthenticated)
            {
                setProgress(0.5f);
                setOutput("Cloud Save Authenticated");

                yield return cloudSaveRecord.ReadDefaultSaveGameAsync(
                    (saveDataString) =>
                    {
                        setOutput("Default Cloud Save Loaded");
                    },
                    (status) =>
                    {
                        setOutput($"Cloud Save Not Loaded ({status})");
                    });
            }
            else
            {
                setOutput("Cloud Save Authentication failed");
            }

            setProgress(1.0f);
        }
    }
}
