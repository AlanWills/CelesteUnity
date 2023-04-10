using Celeste.Notifications.Objects;
using System;
using System.Collections;

namespace Celeste.Notifications
{
    public enum NotificationStatus
    {
        Unavailable = -1,
        Unknown = 0,
        Scheduled = 1,
        Delivered = 2
    }

    public interface INotificationSystem
    {
        bool HasPermissions { get; }
        string LastRespondedNotificationData { get; }

        bool Initialize();
        IEnumerator RequestPermissions();
        NotificationStatus GetNotificationStatus(Notification notification);

        void AddNotificationChannel(NotificationChannel notificationChannel);
        void ScheduleNotification(Notification notification, DateTime dateTime, string intentData);
        void CancelNotification(Notification notification);
        void CancelAllNotifications();
    }
}