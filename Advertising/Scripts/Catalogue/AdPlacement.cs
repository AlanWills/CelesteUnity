using System;
using UnityEngine;

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

        public AdPlacementType PlacementType => placementType;
        public bool IsLoaded { get; set; }
        public Action<AdWatchResult> OnShow { get; set; }

        [SerializeField] private string androidPlacementId;
        [SerializeField] private string iOSPlacementId;
        [SerializeField] private AdPlacementType placementType;

        #endregion
    }
}
