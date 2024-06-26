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

        [SerializeField] private NotificationRecord notificationRecord;

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            if (notificationRecord.Initialize())
            {
                UnityEngine.Debug.Log("Successfully initialized notifications", CelesteLog.Notifications);
                notificationRecord.AddAllNotificationChannels();
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
            foreach (NotificationChannelDTO notificationChannelDTO in dto.notificationChannels)
            {
                notificationRecord.SetNotificationChannelEnabled(notificationChannelDTO.guid, notificationChannelDTO.enabled);
            }
        }

        protected override void SetDefaultValues()
        {
        }

        #endregion
    }
}