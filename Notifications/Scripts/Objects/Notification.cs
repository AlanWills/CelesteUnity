using Celeste.Objects;
using UnityEngine;

namespace Celeste.Notifications.Objects
{
    [CreateAssetMenu(fileName = nameof(Notification), menuName = "Celeste/Notifications/Notification")]
    public class Notification : ScriptableObject, IGuid
    {
        #region Properties and Fields

        int IGuid.Guid
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

        public int ID => guid;
        public string Title => title;
        public string SubTitle => subTitle;
        public string Text => text;
        public string SmallIcon => smallIcon;
        public string LargeIcon => largeIcon;
        public string NotificationChannelID => notificationChannel.ID;

        [SerializeField] private int guid;
        [SerializeField] private string title;
        [SerializeField] private string subTitle;
        [SerializeField] private string text;
        [SerializeField] private string smallIcon;
        [SerializeField] private string largeIcon;
        [SerializeField] private NotificationChannel notificationChannel;

        #endregion
    }
}