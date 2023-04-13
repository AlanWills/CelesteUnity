#if UNITY_IOS
using System;
using System.Collections;
using Celeste.Notifications.Objects;
using Unity.Notifications.iOS;

namespace Celeste.Notifications.Impls
{
    public class IOSNotificationSystem : INotificationSystem
    {
        #region Properties and Fields

        public bool PermissionsRequested => iOSNotificationCenter.GetNotificationSettings().AuthorizationStatus == AuthorizationStatus.NotDetermined;
        public bool PermissionsGranted => iOSNotificationCenter.GetNotificationSettings().AuthorizationStatus == AuthorizationStatus.Authorized;

        public string LastRespondedNotificationData
        {
            get
            {
                var notification = iOSNotificationCenter.GetLastRespondedNotification();
                return notification != null ? notification.Data : string.Empty;
            }
        }

        #endregion
        
        public bool Initialize()
        {
            return true;
        }

        public IEnumerator RequestPermissions()
        {
            var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
            using (var req = new AuthorizationRequest(authorizationOption, true))
            {
                while (!req.IsFinished)
                {
                    yield return null;
                };

                string res = "\n RequestAuthorization:";
                res += "\n finished: " + req.IsFinished;
                res += "\n granted :  " + req.Granted;
                res += "\n error:  " + req.Error;
                res += "\n deviceToken:  " + req.DeviceToken;
                UnityEngine.Debug.Log(res);
            }
        }

        public void ResetPermissions()
        {
            UnityEngine.Debug.LogAssertion($"Cannot reset permissions on iOS yet.");
        }

        public NotificationStatus GetNotificationStatus(Notification notification)
        {
            string notificationId = notification.ID.ToString();
            var scheduledNotifications = iOSNotificationCenter.GetScheduledNotifications();

            if (scheduledNotifications != null && Array.Exists(scheduledNotifications, x => x.Identifier == notificationId))
            {
                return NotificationStatus.Scheduled;
            }

            var deliveredNotifications = iOSNotificationCenter.GetDeliveredNotifications();

            if (deliveredNotifications != null && Array.Exists(deliveredNotifications, x => x.Identifier == notificationId))
            {
                return NotificationStatus.Delivered;
            }

            return NotificationStatus.Unknown;
        }

        public void AddNotificationChannel(NotificationChannel notificationChannel)
        {

        }

        public void ScheduleNotification(Notification notification, DateTime dateTime, string intentData)
        {
            var dateTimeTrigger = CreateCalendarTrigger(dateTime);
            string notificationID = notification.ID.ToString();

            var iOSNotification = new iOSNotification()
            {
                // You can specify a custom identifier which can be used to manage the notification later.
                // If you don't provide one, a unique string will be generated automatically.
                Identifier = notificationID,
                Title = notification.Title,
                Subtitle = notification.SubTitle,
                Body = notification.Text,
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = notification.NotificationChannelID,
                ThreadIdentifier = "thread1",
                Trigger = dateTimeTrigger,
                Data = intentData
            };

            NotificationStatus notificationStatus = GetNotificationStatus(notification);

            if (notificationStatus == NotificationStatus.Scheduled ||
                notificationStatus == NotificationStatus.Delivered)
            {
                CancelNotification(notification);
            }
            
            SendNotification(iOSNotification);
        }

        public void CancelNotification(Notification notification)
        {
            string notificationID = notification.ID.ToString();
            iOSNotificationCenter.RemoveScheduledNotification(notificationID);
            iOSNotificationCenter.RemoveDeliveredNotification(notificationID);
        }

        public void CancelAllNotifications()
        {
            iOSNotificationCenter.RemoveAllScheduledNotifications();
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
        }

        private iOSNotificationCalendarTrigger CreateCalendarTrigger(DateTime dateTime)
        {
            return new iOSNotificationCalendarTrigger()
            {
                Year = dateTime.Year,
                Month = dateTime.Month,
                Day = dateTime.Day,
                Hour = dateTime.Hour,
                Minute = dateTime.Minute,
                Second = dateTime.Second,
                Repeats = false
            };
        }

        private void SendNotification(iOSNotification notification)
        {
            iOSNotificationCenter.ScheduleNotification(notification);
        }
    }
}
#endif