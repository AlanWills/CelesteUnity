using Celeste.Coroutines;
using Celeste.Debug.Menus;
using Celeste.Notifications.Catalogue;
using Celeste.Notifications.Record;
using Celeste.Tools;
using System;
using UnityEngine;

namespace Celeste.Notifications.Debug
{
    [CreateAssetMenu(fileName = nameof(NotificationDebugMenu), menuName = "Celeste/Notifications/Debug/Notification Debug Menu")]
    public class NotificationDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private NotificationRecord notificationRecord;
        [SerializeField] private NotificationChannelCatalogue notificationChannelCatalogue;
        [SerializeField] private NotificationCatalogue notificationCatalogue;

        [NonSerialized] private int currentNotificationsChannelPage;
        [NonSerialized] private int currentNotificationsPage;

        private const int NOTIFICATIONS_CHANNEL_PER_PAGE = 20;
        private const int NOTIFICATIONS_PER_PAGE = 20;

        #endregion

        protected override void OnDrawMenu()
        {
            GUILayout.Label($"Permissions Requested: {notificationRecord.PermissionsRequested}");
            GUILayout.Label($"Permissions Granted: {notificationRecord.PermissionsGranted}");

            using (var horizontal = new GUILayout.HorizontalScope())
            {
                using (new GUIEnabledScope(!notificationRecord.PermissionsRequested))
                {
                    if (GUILayout.Button("Request"))
                    {
                        CoroutineManager.Instance.StartCoroutine(notificationRecord.RequestPermissions());
                    }
                }

                using (new GUIEnabledScope(notificationRecord.PermissionsRequested))
                {
                    if (GUILayout.Button("Reset"))
                    {
                        notificationRecord.ResetPermissions();
                    }
                }
            }

            if (GUILayout.Button("Add All Notification Channels"))
            {
                notificationRecord.AddAllNotificationChannels();
            }

            if (GUILayout.Button("Cancel All Notifications"))
            {
                notificationRecord.CancelAllNotifications();
            }

            currentNotificationsChannelPage = GUIUtils.ReadOnlyPaginatedList(
                currentNotificationsChannelPage,
                NOTIFICATIONS_CHANNEL_PER_PAGE,
                notificationCatalogue.NumItems,
                (index) =>
                {
                    var notification = notificationCatalogue.GetItem(index);
                    var notificationStatus = notificationRecord.GetNotificationStatus(notification);

                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label($"{notification.name} ({notification.ID}) - {notificationStatus}");

                        if (GUILayout.Button("Schedule"))
                        {
                            notificationRecord.ScheduleNotification(
                                notification,
                                DateTime.Now.AddSeconds(30),
                                string.Empty);
                        }

                        if (GUILayout.Button("Cancel"))
                        {
                            notificationRecord.CancelNotification(notification);
                        }
                    }
                });

            currentNotificationsPage = GUIUtils.ReadOnlyPaginatedList(
                currentNotificationsPage,
                NOTIFICATIONS_PER_PAGE,
                notificationChannelCatalogue.NumItems,
                (index) =>
                {
                    var notificationChannel = notificationChannelCatalogue.GetItem(index);
                    notificationChannel.Enabled = GUILayout.Toggle(
                        notificationChannel.Enabled,
                        $"{notificationChannel.name} ({notificationChannel.ID})");
                });
        }
    }
}
