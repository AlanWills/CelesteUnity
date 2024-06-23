#if UNITY_ANDROID && UNITY_NOTIFICATIONS
using Celeste.Notifications.Objects;
using System;
using System.Collections;
using Unity.Notifications.Android;

namespace Celeste.Notifications.Impls
{
    public class AndroidNotificationSystem : INotificationSystem
    {
        #region Properties and Fields

        public bool PermissionsRequested => AndroidNotificationCenter.UserPermissionToPost != PermissionStatus.NotRequested;
        public bool PermissionsGranted => AndroidNotificationCenter.UserPermissionToPost == PermissionStatus.Allowed;
        
        public string LastRespondedNotificationData
        {
            get
            {
                var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
                return notificationIntentData != null ? notificationIntentData.Notification.IntentData : string.Empty;
            }
        }

        #endregion

        public bool Initialize()
        {
            return AndroidNotificationCenter.Initialize();
        }

        public IEnumerator RequestPermissions()
        {
            UnityEngine.Debug.Log($"{nameof(AndroidNotificationCenter.UserPermissionToPost)}: {AndroidNotificationCenter.UserPermissionToPost}", CelesteLog.Notifications);
            PermissionRequest permissionRequest = new PermissionRequest();

            while (permissionRequest.Status == PermissionStatus.RequestPending)
            {
                yield return null;
            }

            UnityEngine.Debug.Log($"Permission Request finished with status: {permissionRequest.Status}", CelesteLog.Notifications);
            UnityEngine.Debug.Log($"{nameof(AndroidNotificationCenter.UserPermissionToPost)} after request: {AndroidNotificationCenter.UserPermissionToPost}", CelesteLog.Notifications);
        }

        public void OpenPermissionsSettings()
        {
            AndroidNotificationCenter.OpenNotificationSettings();
        }
        
        public NotificationStatus GetNotificationStatus(Notification notification)
        {
            var status = AndroidNotificationCenter.CheckScheduledNotificationStatus(notification.ID);

            switch (status)
            {
                case Unity.Notifications.Android.NotificationStatus.Unavailable:
                    return NotificationStatus.Unavailable;

                case Unity.Notifications.Android.NotificationStatus.Unknown:
                    return NotificationStatus.Unknown;

                case Unity.Notifications.Android.NotificationStatus.Scheduled:
                    return NotificationStatus.Scheduled;

                case Unity.Notifications.Android.NotificationStatus.Delivered:
                    return NotificationStatus.Delivered;

                default:
                    UnityEngine.Debug.LogAssertion($"Failed to convert android notification status '{status}' into a celeste notification status.", CelesteLog.Notifications);
                    return NotificationStatus.Unknown;
            }
        }

        public void AddNotificationChannel(NotificationChannel notificationChannel)
        {
            var channel = new AndroidNotificationChannel
            {
                Id = notificationChannel.ID,
                Name = notificationChannel.name,
                Importance = ToAndroidImportance(notificationChannel.Importance),
                Description = notificationChannel.Description,
                CanShowBadge = notificationChannel.CanShowBadge
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);
        }

        public void ScheduleNotification(Notification notification, DateTimeOffset dateTimeOffset, string intentData)
        {
            var androidNotification = new AndroidNotification()
            {
                Title = notification.Title,
                Text = notification.Text,
                FireTime = dateTimeOffset.DateTime,
                SmallIcon = notification.SmallIcon,
                LargeIcon = notification.LargeIcon,
                IntentData = intentData,
                RepeatInterval = notification.IsRepeating ? TimeSpan.FromSeconds(notification.RepeatTimeInSeconds) : null,
                ShowTimestamp = notification.ShowTimestamp,
                Group = notification.NotificationChannelID,
                Number = notification.Number
            };

            int notificationId = notification.ID;
            string notificationChannelId = notification.NotificationChannelID;
            NotificationStatus notificationStatus = GetNotificationStatus(notification);
            UnityEngine.Debug.Log($"Notification with id has status {notificationStatus} before attempted scheduling.", CelesteLog.Notifications);

            if (notificationStatus == NotificationStatus.Scheduled)
            {
                // Replace the scheduled notification with a new notification.
                CancelNotification(notification);
                SendNotification(androidNotification, notificationChannelId, notificationId);
                UnityEngine.Debug.Log($"Notification with id {notificationId} was scheduled and has been updated.", CelesteLog.Notifications);
            }
            else if (notificationStatus == NotificationStatus.Delivered)
            {
                CancelNotification(notification);
                SendNotification(androidNotification, notificationChannelId, notificationId);
                UnityEngine.Debug.Log($"Notification with id {notificationId} was delivered, so has been cancelled and updated.", CelesteLog.Notifications);
            }
            else if (notificationStatus == NotificationStatus.Unknown)
            {
                SendNotification(androidNotification, notificationChannelId, notificationId);
                UnityEngine.Debug.Log($"Notification with id {notificationId} had an unknown status and has been resent.", CelesteLog.Notifications);
            }
            else if (notificationStatus == NotificationStatus.Unknown)
            {
                UnityEngine.Debug.LogError($"Notification with id {notificationId} could not be scheduled due to unknown status.", CelesteLog.Notifications);
            }
        }

        public void CancelNotification(Notification notification)
        {
            AndroidNotificationCenter.CancelNotification(notification.ID);
        }

        public void CancelAllNotifications()
        {
            AndroidNotificationCenter.CancelAllNotifications();
        }

        private void SendNotification(
            AndroidNotification androidNotification,
            string notificationChannelId,
            int notificationId)
        {
            AndroidNotificationCenter.SendNotificationWithExplicitID(androidNotification, notificationChannelId, notificationId);
        }

        private static Importance ToAndroidImportance(NotificationChannelImportance importance)
        {
            switch (importance)
            {
                case NotificationChannelImportance.Low:
                    return Importance.Low;

                case NotificationChannelImportance.None:
                    return Importance.None;

                case NotificationChannelImportance.Default:
                    return Importance.Default;

                case NotificationChannelImportance.High:
                    return Importance.High;

                default:
                    UnityEngine.Debug.LogAssertion($"Failed to convert celeste importance '{importance}' to android importance enum.", CelesteLog.Notifications);
                    return Importance.None;
            }
        }
    }
}
#endif