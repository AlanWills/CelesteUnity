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

        [SerializeField] private SectionLogSettings coreLogSettings;
        [SerializeField] private SectionLogSettings persistenceLogSettings;
        [SerializeField] private SectionLogSettings addressablesLogSettings;

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