using Celeste.Loading;
using Celeste.Log;
using Celeste.Notifications.Record;
using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Notifications.Loading
{
    [CreateAssetMenu(fileName = nameof(RequestNotificationPermissionsLoadJob), menuName = "Celeste/Notifications/Loading/Request Notification Permissions")]
    public class RequestNotificationPermissionsLoadJob : LoadJob
    {
        [SerializeField] private NotificationRecord notificationRecord;

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            yield return notificationRecord.RequestPermissions();

            HudLog.LogInfo($"Notification Permissions? {notificationRecord.HasPermissions}");
            if (notificationRecord.HasPermissions)
            {
                notificationRecord.AddAllNotificationChannels();
            }
        }
    }
}
