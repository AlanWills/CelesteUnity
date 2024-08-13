using Celeste.Events;
using Celeste.Objects;
using Celeste.Parameters;
using Celeste.Tools;
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

    [CreateAssetMenu(fileName = nameof(NotificationChannel), menuName = CelesteMenuItemConstants.NOTIFICATIONS_MENU_ITEM + "Notification Channel", order = CelesteMenuItemConstants.NOTIFICATIONS_MENU_ITEM_PRIORITY)]
    public class NotificationChannel : ScriptableObject, IIntGuid
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
        public string DisplayName => displayName;
        public string Description => description;
        public bool CanShowBadge => canShowBadge;
        public bool IsDebugOnly => isDebugOnly;

        [SerializeField] private int guid;
        [SerializeField] private BoolValue enabled;
        [SerializeField] private NotificationChannelImportance importance;
        [SerializeField] private string displayName;
        [SerializeField] private string description;
        [SerializeField] private bool canShowBadge = true;
        [SerializeField] private bool isDebugOnly = false;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(displayName))
            {
                displayName = name;
                EditorOnly.SetDirty(this);
            }
        }

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