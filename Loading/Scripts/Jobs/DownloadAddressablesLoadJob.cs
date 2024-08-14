#if USE_ADDRESSABLES
using Celeste.Assets;
using Celeste.Tools;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Celeste.Loading
{
    [CreateAssetMenu(fileName = nameof(DownloadAddressablesLoadJob), menuName = CelesteMenuItemConstants.LOADING_MENU_ITEM + "Load Jobs/Download Addressables", order = CelesteMenuItemConstants.LOADING_MENU_ITEM_PRIORITY)]
    public class DownloadAddressablesLoadJob : LoadJob
    {
        #region Properties and Fields

        public string AddressablesLabel
        {
            get => addressablesLabel;
            set
            {
                if (string.CompareOrdinal(addressablesLabel, value) != 0)
                {
                    addressablesLabel = value;
                    EditorOnly.SetDirty(this);
                }
            }
        }

        [Tooltip("Download all of the assets marked with this addressable label.  This is not downloading a particular addressable group, but rather all assets with this label.")]
        [SerializeField] private string addressablesLabel;
        [Tooltip("Should we still attempt to download addressables if no internet connection is detected, or skip the process entirely?")]
        [SerializeField] private bool skipWithNoInternetConnection = true;

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            if (skipWithNoInternetConnection && Application.internetReachability == NetworkReachability.NotReachable)
            {
                setOutput($"Skipping download of {addressablesLabel} due to no internet connection.");
                setProgress(1);
                yield break;
            }

            // Check the download size
            {
                Debug.Assert(!string.IsNullOrEmpty(addressablesLabel), $"No addressables key set for download job.", this);
                AsyncOperationHandle<long> getSizeOperation = Addressables.GetDownloadSizeAsync(addressablesLabel);
                
                using (var operationWrapper = new AddressableOperationHandleWrapper<long>(getSizeOperation))
                {
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
                            Debug.LogError(getSizeOperation.OperationException.Message, CelesteLog.Addressables);
                            setOutput($"Failed: Could not get size of {addressablesLabel}");
                            setProgress(1);

                            yield break;
                        }

                        Debug.Log($"Size of {addressablesLabel} is {getSizeOperation.Result}.", CelesteLog.Addressables);

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
                }
            }

            // Download if necessary
            {
                AsyncOperationHandle downloadOperation = Addressables.DownloadDependenciesAsync(addressablesLabel);

                using (var operationWrapper = new AddressableOperationHandleWrapper(downloadOperation))
                {
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
                            Debug.LogError(downloadOperation.OperationException.Message, CelesteLog.Addressables);
                            setOutput($"Failed: Could not download {addressablesLabel}");
                            setProgress(1);

                            yield break;
                        }

                        Debug.Log($"Downloaded {addressablesLabel} successfully!", CelesteLog.Addressables);
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
    }
}
#endif