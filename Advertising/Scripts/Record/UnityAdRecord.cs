using System;
using UnityEngine;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

namespace Celeste.Advertising
{
    [CreateAssetMenu(fileName = nameof(UnityAdRecord), menuName = "Celeste/Advertising/Unity Ad Record")]
    public class UnityAdRecord : AdRecord
#if UNITY_ADS
        , IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
#endif
    {
        public override void Initialize(bool testMode)
        {
#if UNITY_ADS
            Advertisement.Initialize(GameId, testMode, this);
#endif
        }

        public override void LoadAdPlacement(AdPlacement adPlacement)
        {
#if UNITY_ADS
            Advertisement.Load(adPlacement.PlacementId, this);
#endif
        }

        public override void PlayAdPlacement(AdPlacement adPlacement, Action<AdWatchResult> onShowSuccessful)
        {
#if UNITY_ADS
            adPlacement.OnShow += onShowSuccessful;
            Advertisement.Show(adPlacement.PlacementId, this);
#endif
        }

#if UNITY_ADS
        #region IUnityAdsInitializationListener

        public void OnInitializationComplete()
        {
            UnityEngine.Debug.Log($"Initialization of ads was completed successfully.");
            LoadAllAdPlacements();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            UnityEngine.Debug.LogError($"Initialization of ads failed with message {message} and error {error}.");
        }

        #endregion

        #region IUnityAdsLoadListener

        public void OnUnityAdsAdLoaded(string placementId)
        {
            UnityEngine.Debug.Log($"Loading of placement with id {placementId} was completely successfully.");
            AdPlacement adPlacement = FindAdPlacement(placementId);
            UnityEngine.Debug.Assert(adPlacement != null, $"Could not find ad placement {placementId}.");

            if (adPlacement != null)
            {
                adPlacement.IsLoaded = true;
            }
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            UnityEngine.Debug.Log($"Loading of placement with id {placementId} failed with message {message} and error {error}.");
            AdPlacement adPlacement = FindAdPlacement(placementId);
            UnityEngine.Debug.Assert(adPlacement != null, $"Could not find ad placement {placementId}.");

            if (adPlacement != null)
            {
                adPlacement.IsLoaded = false;
            }
        }

        #endregion

        #region IUnityAdsShowListener

        public void OnUnityAdsShowClick(string placementId)
        {
        }

        public void OnUnityAdsShowStart(string placementId)
        {
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            UnityEngine.Debug.Log($"Showing {placementId} was completed with state {showCompletionState}.");
            AdPlacement adPlacement = FindAdPlacement(placementId);
            UnityEngine.Debug.Assert(adPlacement != null, $"Failed to find placement with id {placementId}.");

            if (adPlacement != null)
            {
                if (adPlacement.OnShow != null)
                {
                    adPlacement.OnShow(ToAdWatchResult(showCompletionState));
                    adPlacement.OnShow = null;
                }

                LoadAdPlacement(adPlacement);
            }
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            UnityEngine.Debug.LogError($"Showing {placementId} failed due to error {error} - {message}.");
            AdPlacement adPlacement = FindAdPlacement(placementId);
            UnityEngine.Debug.Assert(adPlacement != null, $"Failed to find placement with id {placementId}.");

            if (adPlacement != null)
            {
                if (adPlacement.OnShow != null)
                {
                    adPlacement.OnShow(ToAdWatchResult(error));
                    adPlacement.OnShow = null;
                }

                LoadAdPlacement(adPlacement);
            }
        }

        #endregion

        private AdWatchResult ToAdWatchResult(UnityAdsShowCompletionState showCompleteState)
        {
            switch (showCompleteState)
            {
                case UnityAdsShowCompletionState.SKIPPED:
                    return AdWatchResult.Skipped;

                case UnityAdsShowCompletionState.COMPLETED:
                    return AdWatchResult.Completed;

                case UnityAdsShowCompletionState.UNKNOWN:
                default:
                    return AdWatchResult.Unknown;
            }
        }

        private AdWatchResult ToAdWatchResult(UnityAdsShowError showError)
        {
            switch (showError)
            {
                case UnityAdsShowError.ALREADY_SHOWING:
                    return AdWatchResult.Failed_AlreadyShowing;

                case UnityAdsShowError.INTERNAL_ERROR:
                    return AdWatchResult.Failed_InternalError;

                case UnityAdsShowError.INVALID_ARGUMENT:
                    return AdWatchResult.Failed_InvalidArgument;

                case UnityAdsShowError.NOT_INITIALIZED:
                    return AdWatchResult.Failed_NotInitialized;

                case UnityAdsShowError.NOT_READY:
                    return AdWatchResult.Failed_NotReady;

                case UnityAdsShowError.NO_CONNECTION:
                    return AdWatchResult.Failed_NoConnection;

                case UnityAdsShowError.VIDEO_PLAYER_ERROR:
                    return AdWatchResult.Failed_VideoPlayerError;

                case UnityAdsShowError.UNKNOWN:
                default:
                    return AdWatchResult.Unknown;
            }
        }
#endif
    }
}
