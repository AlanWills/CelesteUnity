using UnityEngine;
using Celeste.Persistence;
using Celeste.Notifications.Persistence;
using Celeste.Notifications.Record;
using Celeste.Events;

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
                UnityEngine.Debug.Log("Successfully initialized notifications", CelesteLog.Notifications);
            }
            else
            {
                UnityEngine.Debug.Log("Notifications failed to initialize", CelesteLog.Notifications);
            }
        }

        #endregion

        #region Save/Load

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

        #endregion
    }
}