using Celeste.Loading;
using System;
using System.Collections;
using UnityEngine;
#if UNITY_IOS && UNITY_ATT
using Unity.Advertisement.IosSupport;
#endif

namespace Celeste.Advertising
{
#if UNITY_ATT
    [CreateAssetMenu(fileName = nameof(RequestATTPermissionLoadJob), menuName = CelesteMenuItemConstants.ADVERTISING_MENU_ITEM + "Loading/Request ATT Permission", order = CelesteMenuItemConstants.ADVERTISING_MENU_ITEM_PRIORITY)]
#endif
    public class RequestATTPermissionLoadJob : LoadJob
    {
        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
#if UNITY_IOS && UNITY_ATT
            // Check the user's consent status.
            // If the status is undetermined, display the request dialogue:
            var currentTrackingStatus = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            UnityEngine.Debug.Log($"Current ATT tracking status: {currentTrackingStatus}.");

            if (currentTrackingStatus == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                setProgress(0.5f);
                setOutput("Requesting ATT Authorization");
                ATTrackingStatusBinding.RequestAuthorizationTracking();
            }
#endif
            setProgress(1f);
            yield break;
        }
    }
}