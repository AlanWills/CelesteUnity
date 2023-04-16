using Celeste.Events;
using Celeste.Objects;
using Celeste.Parameters;
using UnityEngine;
using UnityEngine.Events;

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

        public bool Enabled
        {
            get => enabled.Value;
            set => enabled.Value = value;
        }

        public string ID => guid.ToString();
        public NotificationChannelImportance Importance => importance;
        public string Description => description;

        [SerializeField] private int guid;
        [SerializeField] private BoolValue enabled;
        [SerializeField] private NotificationChannelImportance importance;
        [SerializeField] private string description;

        #endregion

        #region Callbacks

        public void AddEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback)
        {
            enabled.AddValueChangedCallback(callback);
        }

        public void RemoveEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback)
        {
            enabled.RemoveValueChangedCallback(callback);
        }

        #endregion
    }
}