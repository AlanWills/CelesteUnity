using System;
using System.Collections;
using System.Collections.Generic;
using Celeste.Notifications.Interfaces;
using Unity.Notifications.iOS;
using UnityEngine;

namespace Celeste.Notifications.Impls
{
    public class IOSNotificationSystem : INotificationSystem
    {
        public bool HasNotificationsPermissions =>
            iOSNotificationCenter.GetNotificationSettings().AuthorizationStatus == AuthorizationStatus.Authorized;
        
        public IEnumerator RequestAuthorization()
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
                Debug.Log(res);
            }
        }

        public void ScheduleNotification()
        {
            var timeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = new TimeSpan(0, minutes, seconds),
                Repeats = false
            };

            var notification = new iOSNotification()
            {
                // You can specify a custom identifier which can be used to manage the notification later.
                // If you don't provide one, a unique string will be generated automatically.
                Identifier = "_notification_01",
                Title = "Title",
                Body = "Scheduled at: " + DateTime.Now.ToShortDateString() + " triggered in 5 seconds",
                Subtitle = "This is a subtitle, something, something important...",
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread1",
                Trigger = timeTrigger,
            };
            notification.Data = "{\"title\": \"Notification 1\", \"data\": \"200\"}";
            
            iOSNotificationCenter.ScheduleNotification(notification);

            // The following code example cancels a notification if it doesn’t trigger.
            //iOSNotificationCenter.GetScheduledNotifications();
            //iOSNotificationCenter.RemoveScheduledNotification(notification.Identifier);

            // The following code example removes a notification from the Notification Center if it's already delivered.
            //iOSNotificationCenter.GetDeliveredNotifications();
            //iOSNotificationCenter.RemoveDeliveredNotification(notification.Identifier);
        }

        public void GetLastRespondedNotification()
        {
            var notification = iOSNotificationCenter.GetLastRespondedNotification();
            if (notification != null)
            {
                var msg = "Last Received Notification: " + notification.Identifier;
                msg += "\n - Notification received: ";
                msg += "\n - .Title: " + notification.Title;
                msg += "\n - .Badge: " + notification.Badge;
                msg += "\n - .Body: " + notification.Body;
                msg += "\n - .CategoryIdentifier: " + notification.CategoryIdentifier;
                msg += "\n - .Subtitle: " + notification.Subtitle;
                msg += "\n - .Data: " + notification.Data;
                Debug.Log(msg);
            }
        }
        
        private void CreateCalendarTrigger()
        {
            var calendarTrigger = new iOSNotificationCalendarTrigger()
            {
                // Year = 2020,
                // Month = 6,
                //Day = 1,
                Hour = 12,
                Minute = 0,
                // Second = 0
                Repeats = false
            };
        }

        private void CreateLocationTrigger()
        {
            var locationTrigger = new iOSNotificationLocationTrigger()
            {
                Center = new Vector2(2.294498f, 48.858263f),
                Radius = 250f,
                NotifyOnEntry = true,
                NotifyOnExit = false,
            };
        }
    }
}