using Celeste.Notifications.Record;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Notifications.UI
{
    [AddComponentMenu("Celeste/Notifications/UI/Notification Toggle UI")]
    public class NotificationToggleUI : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private NotificationRecord notificationRecord;
        [SerializeField] private Toggle notificationEnabledToggle;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            UpdateUI();
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                UpdateUI();
            }
        }

        #endregion

        private void UpdateUI()
        {
            notificationEnabledToggle.SetIsOnWithoutNotify(notificationRecord.PermissionsGranted);
        }

        private IEnumerator RequestNotificationsCoroutine(bool requestEnableNotifications)
        {
            if (requestEnableNotifications && !notificationRecord.PermissionsRequested)
            {
                yield return notificationRecord.RequestPermissions();
            }
            else
            {
                notificationRecord.OpenPermissionsSettings();
            }
            
            UpdateUI();
        }

        #region Callbacks

        public void OnNotificationsToggled(bool requestEnableNotifications)
        {
            StartCoroutine(RequestNotificationsCoroutine(requestEnableNotifications));
        }

        #endregion
    }
}