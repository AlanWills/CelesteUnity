using Celeste.Notifications.Objects;
using System;
using System.Collections;

namespace Celeste.Notifications.Impls
{
    public class DisabledNotificationSystem : INotificationSystem
    {
        #region Properties and Fields

        public bool HasNotificationsPermissions => false;
        public string LastRespondedNotificationData => string.Empty;

        #endregion

        public IEnumerator RequestAuthorization()
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
