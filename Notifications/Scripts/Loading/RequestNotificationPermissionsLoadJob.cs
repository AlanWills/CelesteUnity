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
            yield return notificationRecord.RequestPermissions();

            UnityEngine.Debug.Log($"Notification Permissions? {notificationRecord.PermissionsGranted}", CelesteLog.Notifications);
            if (notificationRecord.PermissionsGranted)
            {
                notificationRecord.AddAllNotificationChannels();
            }
        }
    }
}
