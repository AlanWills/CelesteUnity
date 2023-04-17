using Celeste.Events;
using Celeste.Notifications.Catalogue;
using Celeste.Notifications.Impls;
using Celeste.Notifications.Objects;
using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Notifications.Record
{
    [CreateAssetMenu(fileName = nameof(NotificationRecord), menuName = "Celeste/Notifications/Notification Record")]
    public class NotificationRecord : ScriptableObject, INotificationSystem
    {
        #region Properties and Fields

        public bool PermissionsRequested => impl.PermissionsRequested;
        public bool PermissionsGranted => impl.PermissionsGranted;
        public string LastRespondedNotificationData => impl.LastRespondedNotificationData;

        public int NumNotificationChannels => notificationChannelCatalogue.NumItems;

        [SerializeField] private NotificationChannelCatalogue notificationChannelCatalogue;
        [SerializeField] private Events.Event save;

        [NonSerialized] private INotificationSystem impl = new DisabledNotificationSystem();

        #endregion

        public bool Initialize()
        {
#if UNITY_EDITOR
            impl = new DisabledNotificationSystem();
#elif UNITY_ANDROID
            impl = new AndroidNotificationSystem();
#elif UNITY_IOS
            impl = new IOSNotificationSystem();
#endif
            return impl.Initialize();
        }

        public IEnumerator RequestPermissions()
        {
            if (!PermissionsRequested)
            {
                yield return impl.RequestPermissions();
            }
        }

        public void ResetPermissions()
        {
            impl.ResetPermissions();
        }

        public NotificationStatus GetNotificationStatus(Notification notification)
        {
            return impl.GetNotificationStatus(notification);
        }

        public void AddNotificationChannel(NotificationChannel notificationChannel)
        {
            impl.AddNotificationChannel(notificationChannel);
            notificationChannel.AddEnabledChangedCallback(OnChannelEnabledChanged);
        }

        public void ScheduleNotification(Notification notification, DateTimeOffset dateTimeOffset, string intentData)
        {
            if (notification.NotificationChannelEnabled)
            {
                impl.ScheduleNotification(notification, dateTimeOffset.ToUniversalTime(), intentData);
            }
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

        #region Callbacks

        private void OnChannelEnabledChanged(ValueChangedArgs<bool> args)
        {
            save.Invoke();
        }

        #endregion
    }
}