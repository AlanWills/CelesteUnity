using Celeste.Objects;
using UnityEngine;

namespace Celeste.Notifications.Objects
{
    public enum NotificationChannelImportance
    {
        None = 0,
        Low = 2,
        Default = 3,
        High = 4
    }

    [CreateAssetMenu(fileName = nameof(NotificationChannel), menuName = "Celeste/Notifications/Notification Channel")]
    public class NotificationChannel : ScriptableObject, IGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get => guid;
            set
            {
                if (guid != value)
                {
                    guid = value;
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(this);
#endif
                }
            }
        }

        public string ID => guid.ToString();
        public NotificationChannelImportance Importance => importance;
        public string Description => description;

        [SerializeField] private int guid;
        [SerializeField] private NotificationChannelImportance importance;
        [SerializeField] private string description;

        #endregion
    }
}