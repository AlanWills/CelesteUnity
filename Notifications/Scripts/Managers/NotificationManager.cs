using UnityEngine;
using Celeste.Persistence;
using Celeste.Notifications.Persistence;
using Celeste.Notifications.Record;
using Celeste.Log;

namespace Celeste.Notifications.Managers
{
    [AddComponentMenu("Celeste/Notifications/Notification Manager")]
    public class NotificationManager : PersistentSceneManager<NotificationManager, NotificationManagerDTO>
    {
        #region Properties and Fields

        public const string FILE_NAME = "NotificationManager.dat";
        protected override string FileName => FILE_NAME;

        [SerializeField] private NotificationRecord notificationRecord;

        #endregion

        #region Unity Methods

        protected override void Start()
        {
            base.Start();

            if (notificationRecord.Initialize())
            {
                HudLog.LogInfo("Successfully initialized notifications");
            }
            else
            {
                HudLog.LogError("Notifications failed to initialize");
            }
        }

        #endregion

        protected override NotificationManagerDTO Serialize()
        {
            return new NotificationManagerDTO(notificationRecord);
        }

        protected override void Deserialize(NotificationManagerDTO dto)
        {
        }

        protected override void SetDefaultValues()
        {
        }
    }
}