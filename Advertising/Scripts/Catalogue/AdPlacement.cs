using Celeste.Events;
using Celeste.Parameters;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Advertising
{
    public enum AdPlacementType
    {
        Rewarded,
        Interstitial,
        Banner
    }

    public enum AdWatchResult
    {
        None,
        Completed,
        Skipped,
        Failed_NotInitialized,
        Failed_NotReady,
        Failed_VideoPlayerError,
        Failed_InvalidArgument,
        Failed_NoConnection,
        Failed_AlreadyShowing,
        Failed_InternalError,
        Unknown,
    }

    [CreateAssetMenu(fileName = nameof(AdPlacement), menuName = "Celeste/Advertising/Ad Placement")]
    public class AdPlacement : ScriptableObject
    {
        #region Properties and Fields

        public string PlacementId
        {
            get
            {
#if UNITY_ANDROID
                return androidPlacementId;
#else
                return iOSPlacementId;
#endif
            }
        }

        public bool IsEnabled
        {
            get
            {
                if (!runtimeIsEnabled.HasValue)
                {
                    runtimeIsEnabled = isEnabled;
                }

                return runtimeIsEnabled.Value;
            }
            set => runtimeIsEnabled = value;
        }

        public bool IsLoaded
        {
            get => IsEnabled && isLoaded.Value;
            set => isLoaded.Value = value;
        }

        public AdPlacementType PlacementType => placementType;
        public Action<AdWatchResult> OnShow { get; set; }

        [SerializeField] private string androidPlacementId;
        [SerializeField] private string iOSPlacementId;
        [SerializeField] private AdPlacementType placementType;
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private BoolValue isLoaded;

        [NonSerialized] private bool? runtimeIsEnabled;

        #endregion

        public void AddIsLoadedChangedCallback(UnityAction<ValueChangedArgs<bool>> isLoadedChanged)
        {
            isLoaded.AddValueChangedCallback(isLoadedChanged);
        }

        public void RemoveIsLoadedChangedCallback(UnityAction<ValueChangedArgs<bool>> isLoadedChanged)
        {
            isLoaded.RemoveValueChangedCallback(isLoadedChanged);
        }
    }
}
