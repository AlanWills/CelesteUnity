using Celeste.Log;
using System;
using UnityEngine;

namespace Celeste
{
    public class CelesteLog : MonoBehaviour
    {
        #region Properties and Fields

        public static SectionLogSettings Core => instance?.coreLogSettings;
        public static SectionLogSettings Persistence => instance?.persistenceLogSettings;
        public static SectionLogSettings Addressables => instance?.addressablesLogSettings;
        public static SectionLogSettings Input => instance?.inputLogSettings;
        public static SectionLogSettings Notifications => instance?.notificationLogSettings;
        public static SectionLogSettings Narrative => instance?.narrativeLogSettings;
        public static SectionLogSettings CloudSave => instance?.cloudSaveLogSettings;
        public static SectionLogSettings RemoteConfig => instance?.remoteConfigLogSettings;

        [SerializeField] private SectionLogSettings coreLogSettings;
        [SerializeField] private SectionLogSettings persistenceLogSettings;
        [SerializeField] private SectionLogSettings addressablesLogSettings;
        [SerializeField] private SectionLogSettings inputLogSettings;
        [SerializeField] private SectionLogSettings notificationLogSettings;
        [SerializeField] private SectionLogSettings narrativeLogSettings;
        [SerializeField] private SectionLogSettings cloudSaveLogSettings;
        [SerializeField] private SectionLogSettings remoteConfigLogSettings;

        [NonSerialized] private static CelesteLog instance;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            Debug.Assert(instance == null, $"An instance of {nameof(CelesteLog)} already exists!  It's likely you have two in the hierarchy somewhere.");
            instance = this;
        }

        #endregion
    }
}