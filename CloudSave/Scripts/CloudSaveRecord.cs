using Celeste.Parameters;
using System;
using System.Collections;
using UnityEngine;

namespace Celeste.CloudSave
{
    [CreateAssetMenu(fileName = nameof(CloudSaveRecord), menuName = "Celeste/Cloud Save/Cloud Save Record")]
    public class CloudSaveRecord : ScriptableObject
    {
        #region Properties and Fields

        public bool IsAuthenticated => impl.IsAuthenticated;

        [SerializeField] private BoolValue isDebugBuild;
        [SerializeField] private string defaultSaveGameName = "DefaultSaveGame";

        [NonSerialized] private ICloudSave impl = new DisabledCloudSave();

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
#if UNITY_ANDROID
            impl = new GooglePlayGamesCloudSave();
#endif
        }

        #endregion

        public void Activate()
        {
            impl.Activate(isDebugBuild.Value);
        }

        public IEnumerator AuthenticateAsync(
            Action onAuthenticateSucceeded = null,
            Action<AuthenticateStatus> onAuthenticateFailed = null)
        {
            bool authenticationComplete = false;

            impl.Authenticate(
                () =>
                {
                    authenticationComplete = true;
                    onAuthenticateSucceeded?.Invoke();
                },
                (status) =>
                {
                    impl = new DisabledCloudSave();
                    authenticationComplete = true;
                    onAuthenticateFailed?.Invoke(status);
                });

            while (!authenticationComplete)
            {
                yield return null;
            }
        }

        public IEnumerator ReadDefaultSaveGameAsync(
            Action<string> onSaveGameReadSucceeded = null,
            Action<SaveRequestStatus> onSaveGameReadFailed = null)
        {
            bool readComplete = false;

            impl.ReadSaveGame(
                defaultSaveGameName,
                (saveDataString) =>
                {
                    readComplete = true;
                    onSaveGameReadSucceeded?.Invoke(saveDataString);
                },
                (status) =>
                {
                    readComplete = true;
                    onSaveGameReadFailed?.Invoke(status);
                });

            while (!readComplete)
            {
                yield return null;
            }
        }

        public IEnumerator WriteDefaultSaveGameAsync(
            string saveData,
            TimeSpan totalPlaytime,
            Action onSaveGameSucceeded = null,
            Action<SaveRequestStatus> onSaveGameFailed = null)
        {
            bool writeComplete = false;

            impl.WriteSaveGame(
                defaultSaveGameName,
                saveData,
                totalPlaytime,
                () =>
                {
                    writeComplete = true;
                    onSaveGameSucceeded?.Invoke();
                },
                (status) =>
                {
                    writeComplete = true;
                    onSaveGameFailed?.Invoke(status);
                });

            while (!writeComplete)
            {
                yield return null;
            }
        }

        public IEnumerator DeleteDefaultSaveGameAsync(
            Action onSaveGameDeletedSucceeded = null,
            Action<SaveRequestStatus> onSaveGameDeletedFailed = null)
        {
            bool deleteComplete = false;

            impl.DeleteSaveGame(
                defaultSaveGameName,
                () =>
                {
                    deleteComplete = true;
                    onSaveGameDeletedSucceeded?.Invoke();
                },
                (status) =>
                {
                    deleteComplete = true;
                    onSaveGameDeletedFailed?.Invoke(status);
                });

            while (!deleteComplete)
            {
                yield return null;
            }
        }
    }
}
