using UnityEngine;
using Celeste.Persistence;
using Celeste.Notifications.Persistence;
using Celeste.Notifications.Record;

namespace Celeste.Notifications.Managers
{
    [AddComponentMenu("Celeste/Notifications/Notification Manager")]
    public class NotificationManager : PersistentSceneManager<NotificationManager, NotificationManagerDTO>
    {
        #region Properties and Fields

        public const string FILE_NAME = "NotificationManager.dat";
        protected override string FileName => FILE_NAME;

        #endregion

        protected override NotificationManagerDTO Serialize()
        {
            return new NotificationManagerDTO();
        }

        protected override void Deserialize(NotificationManagerDTO dto)
        {
        }

        protected override void SetDefaultValues()
        {
        }
    }
}