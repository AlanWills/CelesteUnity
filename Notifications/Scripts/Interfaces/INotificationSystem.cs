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
        bool PermissionsRequested { get; }
        bool PermissionsGranted { get; }
        string LastRespondedNotificationData { get; }

        bool Initialize();
        IEnumerator RequestPermissions();
        void ResetPermissions();
        void DenyPermissions();
        NotificationStatus GetNotificationStatus(Notification notification);

        void AddNotificationChannel(NotificationChannel notificationChannel);
        void ScheduleNotification(Notification notification, DateTimeOffset dateTime, string intentData);
        void CancelNotification(Notification notification);
        void CancelAllNotifications();
    }
}