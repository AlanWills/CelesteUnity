using Celeste.Objects;
using Celeste.Tools.Attributes.GUI;
using UnityEngine;

namespace Celeste.Notifications.Objects
{
    [CreateAssetMenu(fileName = nameof(Notification), menuName = "Celeste/Notifications/Notification")]
    public class Notification : ScriptableObject, IIntGuid
    {
        #region Properties and Fields

        int IIntGuid.Guid
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
        public int Number => number;
        public bool ShowTimestamp => showTimestamp;
        public string SmallIcon => smallIcon;
        public string LargeIcon => largeIcon;
        public bool NotificationChannelEnabled => notificationChannel.Enabled;
        public string NotificationChannelID => notificationChannel.ID;
        public bool IsRepeating => isRepeating;
        public long RepeatTimeInSeconds => repeatTimeInSeconds;

        [SerializeField] private int guid;
        [SerializeField] private string title;
        [SerializeField] private string subTitle;
        [SerializeField] private string text;
        [SerializeField] private int number;
        [SerializeField] private bool showTimestamp = true;
        [SerializeField] private string smallIcon;
        [SerializeField] private string largeIcon;
        [SerializeField] private NotificationChannel notificationChannel;
        [SerializeField] private bool isRepeating;
        [SerializeField, ShowIf(nameof(isRepeating))] private long repeatTimeInSeconds;

        #endregion
    }
}