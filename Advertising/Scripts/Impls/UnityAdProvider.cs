using Celeste.Parameters;
using System;
using UnityEngine;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

namespace Celeste.Advertising.Impls
{
    [CreateAssetMenu(fileName = nameof(UnityAdProvider), menuName = "Celeste/Advertising/Providers/Unity Ad Provider")]
    public class UnityAdProvider : ScriptableObject, IAdProvider
#if UNITY_ADS
        , IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
#endif
    {
        #region Properties and Fields

        public string GameId
        {
            get
            {
#if UNITY_ANDROID
                return androidGameId;
#elif UNITY_IOS
                return iOSGameId;
#else
                return string.Empty;
#endif
            }
        }

        [SerializeField] private string androidGameId;
        [SerializeField] private string iOSGameId;
        [SerializeField] private BoolValue testMode;

        [NonSerialized] private AdPlacementCatalogue adPlacements;
        [NonSerialized] private Action onInitializationComplete;
        [NonSerialized] private Action<string> onInitializationFailed;

#endregion

        public void Initialize(
            AdPlacementCatalogue adPlacements,
            Action onInitializationComplete,
            Action<string> onInitializationFailed)
        {
            this.adPlacements = adPlacements;
            this.onInitializationComplete = onInitializationComplete;
            this.onInitializationFailed = onInitializationFailed;

#if UNITY_ADS
            if (!Advertisement.isInitialized)
            {
                Advertisement.Initialize(GameId, testMode.Value, this);
            }
#endif
        }

        public void LoadAdPlacement(AdPlacement adPlacement)
        {
#if UNITY_ADS
            if (adPlacement.IsEnabled)
            {
                Advertisement.Load(adPlacement.PlacementId, this);
            }
#endif
        }

        public void PlayAdPlacement(AdPlacement adPlacement, Action<AdWatchResult> onShow)
        {
#if UNITY_ADS
            if (adPlacement.IsLoaded)
            {
                adPlacement.OnShow += onShow;
                Advertisement.Show(adPlacement.PlacementId, this);
            }
            else
            {
                onShow.Invoke(AdWatchResult.Failed_NotInitialized);
            }
#endif
        }

        private AdPlacement FindAdPlacement(string placementId)
        {
            return adPlacements.FindPlacementById(placementId);
        }

#if UNITY_ADS
        #region IUnityAdsInitializationListener

        public void OnInitializationComplete()
        {
            UnityEngine.Debug.Log($"Initialization of unity ads was completed successfully.");
            onInitializationComplete?.Invoke();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            UnityEngine.Debug.LogError($"Initialization of unity ads failed with message {message} and error {error}.");
            onInitializationFailed?.Invoke(message);
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
