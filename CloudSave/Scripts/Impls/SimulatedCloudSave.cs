using System;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Celeste.CloudSave
{
    public class SimulatedCloudSave : ICloudSave
    {
        #region Properties and Fields

        public bool IsAuthenticated { get; private set; }
        public UserInformation UserInformation => new UserInformation()
        {
            id = "LocalUserID",
            displayName = "Local User",
            avatarUrl = ""
        };

        #endregion

        public void Activate(bool debugLogging) { }

        public void Authenticate(Action onAuthenticateSucceeded, Action<AuthenticateStatus> onAuthenticateFailed)
        {
            IsAuthenticated = true;
            onAuthenticateSucceeded?.Invoke();
        }

        public void DeleteSaveGame(string saveGameName, Action onSaveGameDeletedSucceeded, Action<SaveRequestStatus> onSaveGameDeletedFailed)
        {
            string saveGamePath = Path.Combine(Application.persistentDataPath, $"{saveGameName}.dat");

            if (File.Exists(saveGamePath))
            {
                File.Delete(saveGamePath);
                onSaveGameDeletedSucceeded?.Invoke();
            }
            else
            {
                onSaveGameDeletedFailed?.Invoke(SaveRequestStatus.InternalError);
            }
        }

        public async void ReadSaveGame(string saveGameName, Action<string> onSaveGameOpenedSucceeded, Action<SaveRequestStatus> onSaveGameOpenedFailed)
        {
            string saveGamePath = Path.Combine(Application.persistentDataPath, $"{saveGameName}.dat");

            if (File.Exists(saveGamePath))
            {
                Thread.Sleep(1000);
                string readAllText = await File.ReadAllTextAsync(saveGamePath);
                onSaveGameOpenedSucceeded?.Invoke(readAllText);
            }
            else
            {
                onSaveGameOpenedFailed?.Invoke(SaveRequestStatus.InternalError);
            }
        }

        public void WriteSaveGame(string saveGameName, string saveData, TimeSpan totalPlaytime, Action onSaveGameSucceeded, Action<SaveRequestStatus> onSaveGameFailed)
        {
            string saveGamePath = Path.Combine(Application.persistentDataPath, $"{saveGameName}.dat");
            File.WriteAllText(saveGamePath, saveData);
            onSaveGameSucceeded?.Invoke();
        }
    }
}