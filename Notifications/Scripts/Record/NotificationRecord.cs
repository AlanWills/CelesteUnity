using Celeste.Events;
using Celeste.Notifications.Catalogue;
using Celeste.Notifications.Impls;
using Celeste.Notifications.Objects;
using Celeste.Parameters;
using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Notifications.Record
{
    [CreateAssetMenu(fileName = nameof(NotificationRecord), menuName = CelesteMenuItemConstants.NOTIFICATIONS_MENU_ITEM + "Notification Record", order = CelesteMenuItemConstants.NOTIFICATIONS_MENU_ITEM_PRIORITY)]
    public class NotificationRecord : ScriptableObject, INotificationSystem
    {
        #region Properties and Fields

        public bool IsInitialized => isInitialized;
        public bool PermissionsRequested => impl.PermissionsRequested;
        public bool PermissionsGranted => impl.PermissionsGranted;
        public string LastRespondedNotificationData => impl.LastRespondedNotificationData;
        public int NumNotificationChannels => notificationChannelCatalogue.NumItems;

        [SerializeField] private NotificationChannelCatalogue notificationChannelCatalogue;
        [SerializeField] private BoolValue isDebugBuild;
        [SerializeField] private Events.Event save;

        [NonSerialized] private bool isInitialized;
        [NonSerialized] private INotificationSystem impl = new DisabledNotificationSystem();

        #endregion

        public bool Initialize()
        {
#if UNITY_EDITOR || !UNITY_NOTIFICATIONS
            impl = new DisabledNotificationSystem();
#elif UNITY_ANDROID
            impl = new AndroidNotificationSystem();
#elif UNITY_IOS
            impl = new IOSNotificationSystem();
#endif
            isInitialized = impl.Initialize();
            UnityEngine.Debug.Assert(isInitialized, "Failed to initialize the notification record!");
            return isInitialized;
        }

        public IEnumerator RequestPermissions()
        {
            if (!PermissionsRequested)
            {
                yield return impl.RequestPermissions();
            }
            else
            {
                UnityEngine.Debug.Log($"Skipping requesting notification permissions as we've already requested before.", CelesteLog.Notifications);
            }
        }

        public void OpenPermissionsSettings()
        {
            impl.OpenPermissionsSettings();
        }

        public NotificationStatus GetNotificationStatus(Notification notification)
        {
            return impl.GetNotificationStatus(notification);
        }

        public void AddNotificationChannel(NotificationChannel notificationChannel)
        {
            if (!isDebugBuild.Value && notificationChannel.IsDebugOnly)
            {
                // Don't add debug only channels when not in debug builds
                return;
            }

            impl.AddNotificationChannel(notificationChannel);
            notificationChannel.AddEnabledChangedCallback(OnChannelEnabledChanged);
        }

        public void SetNotificationChannelEnabled(int notificationChannelGuid, bool isEnabled)
        {
            NotificationChannel notificationChannel = notificationChannelCatalogue.FindItemByGuid(notificationChannelGuid);

            if (notificationChannel != null)
            {
                notificationChannel.Enabled = isEnabled;
            }
            else
            {
                UnityEngine.Debug.LogAssertion($"Failed to change the enabled status of notification channel with guid {notificationChannelGuid} as it could not be found.");
            }
        }

        public void ScheduleNotification(Notification notification, DateTimeOffset dateTimeOffset)
        {
            ScheduleNotification(notification, dateTimeOffset, notification.IntentData);
        }

        public void ScheduleNotification(Notification notification, DateTimeOffset dateTimeOffset, string intentData)
        {
            if (!impl.PermissionsGranted)
            {
                UnityEngine.Debug.Log($"Skipping scheduling of notification {notification.name} due to permissions not granted.", CelesteLog.Notifications);
                return;
            }

            if (!notification.NotificationChannelEnabled)
            {
                UnityEngine.Debug.Log($"Skipping scheduling of notification {notification.name} due to channel {notification.NotificationChannelID} not being enabled.", CelesteLog.Notifications);
                return;
            }

            impl.ScheduleNotification(notification, dateTimeOffset.ToLocalTime(), intentData);
        }

        public void CancelNotification(Notification notification)
        {
            impl.CancelNotification(notification);
        }

        public void CancelAllNotifications()
        {
            impl.CancelAllNotifications();
        }

        public void AddAllNotificationChannels()
        {
            foreach (NotificationChannel channel in notificationChannelCatalogue)
            {
                AddNotificationChannel(channel);
            }
        }

        public NotificationChannel GetNotificationChannel(int index)
        {
            return notificationChannelCatalogue.GetItem(index);
        }

        public bool TryGetLastNotificationIntent(out string data)
        {
            return impl.TryGetLastNotificationIntent(out data);
        }

        #region Callbacks

        private void OnChannelEnabledChanged(ValueChangedArgs<bool> args)
        {
            save.Invoke();
        }

        #endregion
    }
}