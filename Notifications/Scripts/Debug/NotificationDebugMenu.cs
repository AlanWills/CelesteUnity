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
            currentPage = GUIUtils.ReadOnlyPaginatedList(
                currentPage,
                NOTIFICATIONS_PER_PAGE,
                notificationCatalogue.NumItems,
                (index) =>
                {
                    var notification = notificationCatalogue.GetItem(index);

                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label($"{notification.name} ({notification.ID})");

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
