using System;

namespace Celeste.CloudSave
{
    public enum AuthenticateStatus
    {
        Success,
        InternalError,
        Cancelled
    }

    public enum SaveRequestStatus
    {
        Success = 1,
        CloudSaveDisabled = 0,
        TimeoutError = -1,
        InternalError = -2,
        AuthenticationError = -3,
        BadInputError = -4
    }

    public interface ICloudSave
    {
        bool IsAuthenticated { get; }

        void Activate(bool debugLogging);
        void Authenticate(Action onAuthenticateSucceeded, Action<AuthenticateStatus> onAuthenticateFailed);

        void ReadSaveGame(
            string saveGameName,
            Action<string> onSaveGameOpenedSucceeded,
            Action<SaveRequestStatus> onSaveGameOpenedFailed);
        void WriteSaveGame(
            string saveGameName,
            string saveData,
            TimeSpan totalPlaytime,
            Action onSaveGameSucceeded,
            Action<SaveRequestStatus> onSaveGameFailed);
        void DeleteSaveGame(
            string filename,
            Action onSaveGameDeletedSucceeded,
            Action<SaveRequestStatus> onSaveGameDeletedFailed);
    }
}
