using Celeste.Parameters;
using Celeste.Persistence.Snapshots;
using System;
using System.Collections;
using UnityEngine;

namespace Celeste.CloudSave
{
    public enum Implementation
    {
        PlatformAppropriate,
        Disabled,
    }

    [CreateAssetMenu(
        fileName = nameof(CloudSaveRecord), 
        menuName = CelesteMenuItemConstants.CLOUDSAVE_MENU_ITEM + "Cloud Save Record",
        order = CelesteMenuItemConstants.CLOUDSAVE_MENU_ITEM_PRIORITY)]
    public class CloudSaveRecord : ScriptableObject
    {
        #region Properties and Fields

        public bool IsAuthenticated => impl.IsAuthenticated;
        public UserInformation UserInformation => impl.UserInformation;

        public DateTimeOffset PlaytimeStart
        {
            get => playtimeStart;
            set
            {
                if (playtimeStart != value)
                {
                    playtimeStart = value;
                    save.Invoke();
                }
            }
        }
        public Implementation ActiveImplementation
        {
            get => activeImplementation;
            set
            {
                if (activeImplementation != value)
                {
                    activeImplementation = value;
                    save.Invoke();
                }
            }
        }

        [SerializeField] private BoolValue isDebugBuild;
        [SerializeField] private string defaultSaveGameName = "DefaultSaveGame";
        [SerializeField] private CloudSaveLoadedEvent cloudSaveLoaded;
        [SerializeField] private Events.Event save;

        [NonSerialized] private ICloudSave impl = new DisabledCloudSave();
        [NonSerialized] private DateTimeOffset playtimeStart;
        [NonSerialized] private Implementation activeImplementation = Implementation.Disabled;

        #endregion

        public void Initialize(DateTimeOffset playtimeStart, Implementation implementation)
        {
            PlaytimeStart = playtimeStart;
            activeImplementation = implementation;

            switch (implementation)
            {
                case Implementation.Disabled:
                    impl = new DisabledCloudSave();
                    break;

                case Implementation.PlatformAppropriate:
                    impl = new DisabledCloudSave();
#if UNITY_ANDROID && GOOGLE_PLAY_GAMES
                    impl = new GooglePlayGamesCloudSave();
#endif
                    break;
            }

            UnityEngine.Debug.Log("Cloud Save Initialized!", CelesteLog.CloudSave);
        }

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
                UnityEngine.Debug.Log("Cloud Save already authenticated", CelesteLog.CloudSave);
                onAuthenticateSucceeded?.Invoke();
                yield break;
            }

            bool authenticationComplete = false;
            UnityEngine.Debug.Log("Beginning to authenticate cloud save", CelesteLog.CloudSave);

            impl.Authenticate(
                () =>
                {
                    UnityEngine.Debug.Log("Successfully authenticated cloud save", CelesteLog.CloudSave);
                    authenticationComplete = true;
                    onAuthenticateSucceeded?.Invoke();
                },
                (status) =>
                {
                    UnityEngine.Debug.Log("Unsuccessfully authenticated cloud save", CelesteLog.CloudSave);
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
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                UnityEngine.Debug.LogWarning("Internet is not reachable, so reading of Cloud Save will be skipped...", CelesteLog.CloudSave);
                onSaveGameReadFailed?.Invoke(SaveRequestStatus.TimeoutError);
                yield break;
            }

            bool readComplete = false;
            UnityEngine.Debug.Log("Beginning to read default cloud save", CelesteLog.CloudSave);

            impl.ReadSaveGame(
                defaultSaveGameName,
                (saveDataString) =>
                {
                    UnityEngine.Debug.Log("Successfully read default cloud save game", CelesteLog.CloudSave);
                    UnityEngine.Debug.Log($"Cloud save string is: {saveDataString}", CelesteLog.CloudSave);
                    readComplete = true;
                    onSaveGameReadSucceeded?.Invoke(saveDataString);
                },
                (status) =>
                {
                    UnityEngine.Debug.LogWarning("Unsuccessfully read default cloud save game", CelesteLog.CloudSave);
                    readComplete = true;
                    onSaveGameReadFailed?.Invoke(status);
                });

            while (!readComplete)
            {
                yield return null;
            }
        }

        public IEnumerator LoadDefaultSaveGameAsync(
            LoadMode loadMode,
            Action<string> onSaveGameLoadSucceeded = null,
            Action<SaveRequestStatus> onSaveGameLoadFailed = null)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                UnityEngine.Debug.LogWarning("Internet is not reachable, so loading of Cloud Save will be skipped...", CelesteLog.CloudSave);
                onSaveGameLoadFailed?.Invoke(SaveRequestStatus.TimeoutError);
                yield break;
            }

            yield return ReadDefaultSaveGameAsync(
                (string saveDataString) =>
                {
                    DataSnapshot dataSnapshot = CreateInstance<DataSnapshot>();
                    JsonUtility.FromJsonOverwrite(saveDataString, dataSnapshot);

                    dataSnapshot.UnpackItems(loadMode);

                    cloudSaveLoaded?.Invoke(new CloudSaveLoadedArgs() { loadedData = dataSnapshot });
                    onSaveGameLoadSucceeded?.Invoke(saveDataString);
                },
                (SaveRequestStatus status) =>
                {
                    UnityEngine.Debug.LogWarning("Unsuccessfully loaded default cloud save game", CelesteLog.CloudSave);
                    onSaveGameLoadFailed?.Invoke(status);
                });
        }

        public IEnumerator WriteDefaultSaveGameAsync(
            string saveData,
            Action onSaveGameSucceeded = null,
            Action<SaveRequestStatus> onSaveGameFailed = null)
        {
            bool writeComplete = false;
            UnityEngine.Debug.Log("Beginning to write default cloud save", CelesteLog.CloudSave);

            UnityEngine.Debug.Assert(impl != null, $"Impl is null!", CelesteLog.CloudSave);
            impl.WriteSaveGame(
                defaultSaveGameName,
                saveData,
                DateTimeOffset.UtcNow - PlaytimeStart,
                () =>
                {
                    UnityEngine.Debug.Log("Successfully wrote default cloud save game", CelesteLog.CloudSave);
                    writeComplete = true;
                    onSaveGameSucceeded?.Invoke();
                },
                (status) =>
                {
                    UnityEngine.Debug.LogWarning("Unsuccessfully wrote default cloud save game", CelesteLog.CloudSave);
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
            UnityEngine.Debug.Log("Beginning to delete default cloud save", CelesteLog.CloudSave);

            impl.DeleteSaveGame(
                defaultSaveGameName,
                () =>
                {
                    UnityEngine.Debug.Log("Successfully deleted default cloud save game", CelesteLog.CloudSave);
                    deleteComplete = true;
                    onSaveGameDeletedSucceeded?.Invoke();
                },
                (status) =>
                {
                    UnityEngine.Debug.LogWarning("Unsuccessfully deleted default cloud save game", CelesteLog.CloudSave);
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
