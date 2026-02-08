using Celeste.Loading;
using Celeste.Notifications.Record;
using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Notifications.Loading
{
    [CreateAssetMenu(fileName = nameof(RequestNotificationPermissionsLoadJob), menuName = CelesteMenuItemConstants.NOTIFICATIONS_MENU_ITEM + "Loading/Request Notification Permissions", order = CelesteMenuItemConstants.NOTIFICATIONS_MENU_ITEM_PRIORITY)]
    public class RequestNotificationPermissionsLoadJob : LoadJob
    {
        [SerializeField] private NotificationRecord notificationRecord;

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            UnityEngine.Debug.Assert(notificationRecord.IsInitialized, $"Attempting to request notification permissions when the {nameof(NotificationRecord)} is not initialized!", CelesteLog.Notifications);
            UnityEngine.Debug.Log($"Beginning notification permissions request (requested: {notificationRecord.PermissionsRequested} & current permissions: {notificationRecord.PermissionsGranted}).", CelesteLog.Notifications);
            yield return notificationRecord.RequestPermissions();
        }
    }
}
