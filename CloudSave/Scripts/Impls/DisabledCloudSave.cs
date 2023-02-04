using System;

namespace Celeste.CloudSave
{
    public class DisabledCloudSave : ICloudSave
    {
        public bool IsAuthenticated => true;

        public void Activate(bool debugLogging) { }

        public void Authenticate(Action onAuthenticateSucceeded, Action<AuthenticateStatus> onAuthenticateFailed) 
        {
            onAuthenticateFailed.Invoke(AuthenticateStatus.Cancelled);
        }

        public void DeleteSaveGame(string filename, Action onSaveGameDeletedSucceeded, Action<SaveRequestStatus> onSaveGameDeletedFailed) 
        {
            onSaveGameDeletedFailed.Invoke(SaveRequestStatus.CloudSaveDisabled);
        }

        public void ReadSaveGame(string saveGameName, Action<string> onSaveGameOpenedSucceeded, Action<SaveRequestStatus> onSaveGameOpenedFailed)
        {
            onSaveGameOpenedFailed.Invoke(SaveRequestStatus.CloudSaveDisabled);
        }

        public void WriteSaveGame(string saveGameName, string saveData, TimeSpan totalPlaytime, Action onSaveGameSucceeded, Action<SaveRequestStatus> onSaveGameFailed)
        {
            onSaveGameFailed.Invoke(SaveRequestStatus.CloudSaveDisabled);
        }
    }
}
