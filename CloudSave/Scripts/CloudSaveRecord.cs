using Celeste.Log;
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
        public DateTimeOffset PlaytimeStart { get; set; }

        [SerializeField] private BoolValue isDebugBuild;
        [SerializeField] private string defaultSaveGameName = "DefaultSaveGame";

        [NonSerialized] private ICloudSave impl = new DisabledCloudSave();

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
#if UNITY_EDITOR
            impl = new DisabledCloudSave();
#elif UNITY_ANDROID && GOOGLE_PLAY_GAMES
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
            if (impl.IsAuthenticated)
            {
                HudLog.LogInfo("Cloud Save already authenticated.");
                onAuthenticateSucceeded?.Invoke();
                yield break;
            }

            bool authenticationComplete = false;
            HudLog.LogInfo("Beginning to authenticate cloud save.");

            impl.Authenticate(
                () =>
                {
                    HudLog.LogInfo("Successfully authenticated cloud save.");
                    authenticationComplete = true;
                    onAuthenticateSucceeded?.Invoke();
                },
                (status) =>
                {
                    HudLog.LogInfo("Unsuccessfully authenticated cloud save.");
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
            HudLog.LogInfo("Beginning to read default cloud save.");

            impl.ReadSaveGame(
                defaultSaveGameName,
                (saveDataString) =>
                {
                    HudLog.LogWarning("Successfully read default cloud save game.");
                    readComplete = true;
                    onSaveGameReadSucceeded?.Invoke(saveDataString);
                },
                (status) =>
                {
                    HudLog.LogWarning("Unsuccessfully read default cloud save game.");
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
            Action onSaveGameSucceeded = null,
            Action<SaveRequestStatus> onSaveGameFailed = null)
        {
            bool writeComplete = false;
            HudLog.LogInfo("Beginning to write default cloud save.");

            impl.WriteSaveGame(
                defaultSaveGameName,
                saveData,
                DateTimeOffset.UtcNow - PlaytimeStart,
                () =>
                {
                    HudLog.LogInfo("Successfully wrote default cloud save game.");
                    writeComplete = true;
                    onSaveGameSucceeded?.Invoke();
                },
                (status) =>
                {
                    HudLog.LogWarning("Unsuccessfully wrote default cloud save game.");
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
            HudLog.LogInfo("Beginning to delete default cloud save.");

            impl.DeleteSaveGame(
                defaultSaveGameName,
                () =>
                {
                    HudLog.LogInfo("Successfully deleted default cloud save game.");
                    deleteComplete = true;
                    onSaveGameDeletedSucceeded?.Invoke();
                },
                (status) =>
                {
                    HudLog.LogWarning("Unsuccessfully deleted default cloud save game.");
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
