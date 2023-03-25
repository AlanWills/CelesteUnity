using Celeste.Notifications.Interfaces;
using Unity.Notifications.Android;
using UnityEngine.Android;

namespace Celeste.Notifications.Impls
{
    public class AndroidNotificationSystem : INotificationSystem
    {
        #region Properties and Fields
        
        public bool HasNotificationsPermissions => Permission.HasUserAuthorizedPermission(ANDROID_NOTIFICATIONS_PERMISSION);
        
        private const string ANDROID_NOTIFICATIONS_PERMISSION = "android.permission.POST_NOTIFICATIONS";
        
        #endregion

        public void RequestNotificationsPermissions()
        {
            if (!Permission.HasUserAuthorizedPermission(ANDROID_NOTIFICATIONS_PERMISSION))
            {
                Permission.RequestUserPermission(ANDROID_NOTIFICATIONS_PERMISSION);
            }
        }
        
        public void AddNotificationChannel()
        {
            var channel = new AndroidNotificationChannel
            {
                Id = "channel_id",
                Name = "Default Channel",
                Importance = Importance.Default,
                Description = "Generic notifications",
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);
        }

        public void ScheduleNotification(int notificationId)
        {
            var notification = new AndroidNotification();
            notification.Title = "Your Title";
            notification.Text = "Your Text";
            notification.FireTime = System.DateTime.Now.AddMinutes(1);
            notification.SmallIcon = "";
            notification.LargeIcon = "";
            notification.IntentData = "{\"title\": \"Notification 1\", \"data\": \"200\"}";
            
            var notificationStatus = AndroidNotificationCenter.CheckScheduledNotificationStatus(notificationId);

            if (notificationStatus == NotificationStatus.Scheduled)
            {
                // Replace the scheduled notification with a new notification.
                AndroidNotificationCenter.UpdateScheduledNotification(notificationId, notification, "channel_id");
            }
            else if (notificationStatus == NotificationStatus.Delivered)
            {
                // Remove the previously shown notification from the status bar.
                AndroidNotificationCenter.CancelNotification(notificationId);
            }
            else if (notificationStatus == NotificationStatus.Unknown)
            {
                AndroidNotificationCenter.SendNotification(notification, "channel_id");
            }

            AndroidNotificationCenter.SendNotificationWithExplicitID(notification, "channel_id", notificationId);
        }

        public void GetLastNotificationIntent()
        {
            var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
            if (notificationIntentData != null)
            {
                var id = notificationIntentData.Id;
                var channel = notificationIntentData.Channel;
                var notification = notificationIntentData.Notification;
            }
        }
    }
}