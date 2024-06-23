using Celeste.Notifications.Objects;
using System;
using System.Collections;

namespace Celeste.Notifications.Impls
{
    public class DisabledNotificationSystem : INotificationSystem
    {
        #region Properties and Fields

        public bool PermissionsRequested { get; private set; }
        public bool PermissionsGranted { get; private set; }
        public string LastRespondedNotificationData => string.Empty;

        #endregion

        public bool Initialize()
        {
            return true;
        }

        public IEnumerator RequestPermissions()
        {
            PermissionsRequested = true;
            PermissionsGranted = true;

            yield break;
        }

        public void OpenPermissionsSettings() { }

        public NotificationStatus GetNotificationStatus(Notification notification)
        {
            return NotificationStatus.Unavailable;
        }

        public void AddNotificationChannel(NotificationChannel notificationChannel) { }
        public void ScheduleNotification(Notification channel, DateTimeOffset dateTime, string intentData) { }
        public void CancelNotification(Notification channel) { }
        public void CancelAllNotifications() { }
    }
}
