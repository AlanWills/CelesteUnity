using Celeste.Notifications.Managers;
using Celeste.Persistence;
using CelesteEditor.Persistence;
using System;
using UnityEditor;

namespace CelesteEditor.Notifications.Persistence
{
    public static class NotificationsPersistenceMenuItems
    {
        [MenuItem("Celeste/Files/Open Notifications Save", priority = 0)]
        public static void OpenNotificationsSaveMenuItem()
        {
            PersistenceMenuItemUtility.OpenExplorerAtPersistentData();
        }

        [MenuItem("Celeste/Files/Delete Notifications Save", priority = 100)]
        public static void DeleteNotificationsSaveMenuItem()
        {
            PersistenceUtility.DeletePersistentDataFile(NotificationManager.FILE_NAME);
        }
    }
}
