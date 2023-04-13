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
        [SerializeField] private NotificationCatalogue notificationCatalogue;

        [NonSerialized] private int currentPage;

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

            currentPage = GUIUtils.ReadOnlyPaginatedList(
                currentPage,
                NOTIFICATIONS_PER_PAGE,
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
                    }
                });
        }
    }
}
