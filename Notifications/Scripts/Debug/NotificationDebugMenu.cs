using Celeste.Coroutines;
using Celeste.Debug.Menus;
using Celeste.Notifications.Catalogue;
using Celeste.Notifications.Record;
using Celeste.Tools;
using System;
using UnityEngine;

namespace Celeste.Notifications.Debug
{
    [CreateAssetMenu(fileName = nameof(NotificationDebugMenu), menuName = CelesteMenuItemConstants.NOTIFICATIONS_MENU_ITEM + "Debug/Notification Debug Menu", order = CelesteMenuItemConstants.NOTIFICATIONS_MENU_ITEM_PRIORITY)]
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
            GUILayout.Label($"Is Initialized: {notificationRecord.IsInitialized}");
            GUILayout.Label($"Permissions Requested: {notificationRecord.PermissionsRequested}");
            GUILayout.Label($"Permissions Granted: {notificationRecord.PermissionsGranted}");

            using (var horizontal = new GUILayout.HorizontalScope())
            {
                using (new GUIEnabledScope(!notificationRecord.IsInitialized))
                {
                    if (GUILayout.Button("Initialize"))
                    {
                        notificationRecord.Initialize();
                    }
                }

                if (GUILayout.Button("Request"))
                {
                    CoroutineManager.Instance.StartCoroutine(notificationRecord.RequestPermissions());
                }

                if (GUILayout.Button("Open Settings"))
                {
                    notificationRecord.OpenPermissionsSettings();
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

            GUILayout.Label("Notifications", CelesteGUIStyles.BoldLabel);
            GUILayout.Space(4);
            
            currentNotificationsChannelPage = GUIExtensions.ReadOnlyPaginatedList(
                currentNotificationsChannelPage,
                NOTIFICATIONS_CHANNEL_PER_PAGE,
                notificationCatalogue.NumItems,
                (index) =>
                {
                    var notification = notificationCatalogue.GetItem(index);
                    var notificationStatus = notificationRecord.GetNotificationStatus(notification);

                    using (Section(notification.name))
                    {
                        GUILayout.Label($"ID: {notification.ID}");
                        GUILayout.Label($"Status: {notificationStatus}");
                        GUILayout.Label($"Title: {notification.Title}");
                        GUILayout.Label($"Intent Data: {notification.IntentData}");

                        using (new GUILayout.HorizontalScope())
                        {
                            if (GUILayout.Button("Schedule in 30s"))
                            {
                                notificationRecord.ScheduleNotification(notification, DateTime.Now.AddSeconds(30));
                            }

                            if (GUILayout.Button("Cancel"))
                            {
                                notificationRecord.CancelNotification(notification);
                            }
                        }
                    }
                });

            GUILayout.Space(4);
            GUILayout.Label("Notification Channels", CelesteGUIStyles.BoldLabel);
            GUILayout.Space(4);
            
            currentNotificationsPage = GUIExtensions.ReadOnlyPaginatedList(
                currentNotificationsPage,
                NOTIFICATIONS_PER_PAGE,
                notificationChannelCatalogue.NumItems,
                (index) =>
                {
                    var notificationChannel = notificationChannelCatalogue.GetItem(index);

                    using (Section(notificationChannel.DisplayName))
                    {
                        GUILayout.Label($"ID: {notificationChannel.ID}");
                        
                        notificationChannel.Enabled = GUILayout.Toggle(notificationChannel.Enabled, "Enabled");
                    }
                });
        }
    }
}
