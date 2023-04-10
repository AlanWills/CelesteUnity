using Celeste.Notifications.Objects;
using System;
using System.Collections;

namespace Celeste.Notifications.Impls
{
    public class DisabledNotificationSystem : INotificationSystem
    {
        #region Properties and Fields

        public bool HasPermissions => false;
        public string LastRespondedNotificationData => string.Empty;

        #endregion

        public bool Initialize()
        {
            return true;
        }

        public IEnumerator RequestPermissions()
        {
            yield break;
        }

        public NotificationStatus GetNotificationStatus(Notification notification)
        {
            return NotificationStatus.Unavailable;
        }

        public void AddNotificationChannel(NotificationChannel notificationChannel) { }
        public void ScheduleNotification(Notification channel, DateTime dateTime, string intentData) { }
        public void CancelNotification(Notification channel) { }
        public void CancelAllNotifications() { }
    }
}
