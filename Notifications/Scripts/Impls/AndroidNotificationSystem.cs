#if UNITY_ANDROID || true
using Celeste.Notifications.Objects;
using System;
using System.Collections;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Android;

namespace Celeste.Notifications.Impls
{
    public class AndroidNotificationSystem : INotificationSystem
    {
        #region Properties and Fields
        
        public bool HasNotificationsPermissions =>
            !isExplicitPermissionsRequired || Permission.HasUserAuthorizedPermission(ANDROID_NOTIFICATIONS_PERMISSION);
        
        public string LastRespondedNotificationData
        {
            get
            {
                var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
                return notificationIntentData != null ? notificationIntentData.Notification.IntentData : string.Empty;
            }
        }

        private bool isExplicitPermissionsRequired;

        private const int MIN_ANDROID_API_LEVEL_FOR_EXPLICIT_PERMISSIONS = 33;
        private const string ANDROID_NOTIFICATIONS_PERMISSION = "android.permission.POST_NOTIFICATIONS";
        
        #endregion

        public bool Initialize()
        {
            using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                int androidSDKVersion = version.GetStatic<int>("SDK_INT");
                isExplicitPermissionsRequired = androidSDKVersion >= MIN_ANDROID_API_LEVEL_FOR_EXPLICIT_PERMISSIONS;
            }

            return AndroidNotificationCenter.Initialize();
        }

        public IEnumerator RequestAuthorization()
        {
            if (isExplicitPermissionsRequired && !Permission.HasUserAuthorizedPermission(ANDROID_NOTIFICATIONS_PERMISSION))
            {
                Permission.RequestUserPermission(ANDROID_NOTIFICATIONS_PERMISSION);
            }

            yield break;
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
                    UnityEngine.Debug.LogAssertion($"Failed to convert android notification status '{status}' into a celeste notification status.");
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
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);
        }

        public void ScheduleNotification(Notification notification, DateTime dateTime, string intentData)
        {
            var androidNotification = new AndroidNotification()
            {
                Title = notification.Title,
                Text = notification.Text,
                FireTime = dateTime,
                SmallIcon = notification.SmallIcon,
                LargeIcon = notification.LargeIcon,
                IntentData = intentData
            };

            int notificationId = notification.ID;
            string notificationChannelId = notification.NotificationChannelID;
            NotificationStatus notificationStatus = GetNotificationStatus(notification);

            if (notificationStatus == NotificationStatus.Scheduled)
            {
                // Replace the scheduled notification with a new notification.
                AndroidNotificationCenter.UpdateScheduledNotification(notificationId, androidNotification, notificationChannelId);
            }
            else if (notificationStatus == NotificationStatus.Delivered)
            {
                CancelNotification(notification);
                SendNotification(androidNotification, notificationChannelId, notificationId);
            }
            else if (notificationStatus == NotificationStatus.Unknown)
            {
                SendNotification(androidNotification, notificationChannelId, notificationId);
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
                    UnityEngine.Debug.LogAssertion($"Failed to convert celeste importance '{importance}' to android importance enum.");
                    return Importance.None;
            }
        }
    }
}
#endif