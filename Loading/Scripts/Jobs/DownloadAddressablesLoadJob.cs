using Celeste.Log;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Celeste.Loading
{
    [CreateAssetMenu(fileName = nameof(DownloadAddressablesLoadJob), menuName = "Celeste/Loading/Load Jobs/Download Addressables")]
    public class DownloadAddressablesLoadJob : LoadJob
    {
        #region Properties and Fields

        [Tooltip("Download all of the assets marked with this addressable label.  This is not downloading a particular addressable group, but rather all assets with this label.")]
        [SerializeField] private string addressablesLabel;

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            Debug.Assert(!string.IsNullOrEmpty(addressablesLabel), $"No addressables key set for download job.", this);
            AsyncOperationHandle<long> getSizeOperation = Addressables.GetDownloadSizeAsync(addressablesLabel);

            if (getSizeOperation.IsValid())
            {
                while (!getSizeOperation.IsDone)
                {
                    setOutput($"Obtaining size of {addressablesLabel}");
                    setProgress(getSizeOperation.GetDownloadStatus().Percent * 0.5f);    // Make in the range 0 -> 0.5

                    yield return null;
                }

                if (getSizeOperation.Status != AsyncOperationStatus.Succeeded)
                {
                    HudLog.LogError(getSizeOperation.OperationException.Message);
                    setOutput($"Failed: Could not get size of {addressablesLabel}");
                    setProgress(1);

                    yield break;
                }

                HudLog.LogInfo($"Size of {addressablesLabel} is {getSizeOperation.Result}");

                if (getSizeOperation.Result == 0)
                {
                    setOutput($"{addressablesLabel} up to date");
                    setProgress(1);

                    yield break;
                }
            }
            else
            {
                setOutput($"Invalid: Could not get size of {addressablesLabel}");
                setProgress(1);

                yield break;
            }

            AsyncOperationHandle downloadOperation = Addressables.DownloadDependenciesAsync(addressablesLabel);

            if (downloadOperation.IsValid())
            {
                while (!downloadOperation.IsDone)
                {
                    setOutput($"Downloading {addressablesLabel}");
                    setProgress(0.5f + downloadOperation.GetDownloadStatus().Percent * 0.5f);    // Make in the range 0.5 -> 1.0

                    yield return null;
                }

                if (downloadOperation.Status != AsyncOperationStatus.Succeeded)
                {
                    HudLog.LogError(downloadOperation.OperationException.Message);
                    setOutput($"Failed: Could not download {addressablesLabel}");
                    setProgress(1);

                    yield break;
                }

                HudLog.LogInfo($"Downloaded {addressablesLabel} successfully");
            }
            else
            {
                setOutput($"Invalid: Could not download {addressablesLabel}");
                setProgress(1);

                yield break;
            }
        }
    }
}