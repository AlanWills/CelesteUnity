#if UNITY_ANDROID && GOOGLE_PLAY_GAMES
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Text;

namespace Celeste.CloudSave
{
    public class GooglePlayGamesCloudSave : ICloudSave
    {
        #region Properties and Fields

        public bool IsAuthenticated => PlayGamesPlatform.Instance.IsAuthenticated();
        public UserInformation UserInformation
        {
            get
            {
                return IsAuthenticated ? new UserInformation()
                {
                    id = PlayGamesPlatform.Instance.GetUserId(),
                    displayName = PlayGamesPlatform.Instance.GetUserDisplayName(),
                    avatarUrl = PlayGamesPlatform.Instance.GetUserImageUrl()
                } : new UserInformation();
            }
        }

        private bool enableLogging = false;

        #endregion

        #region ICloudSave

        public void Activate(bool debugLogging)
        {
            enableLogging = debugLogging;

            PlayGamesPlatform.DebugLogEnabled = debugLogging;
            PlayGamesPlatform.Activate();
        }

        public void Authenticate(
            Action onAuthenticateSucceeded = null,
            Action<AuthenticateStatus> onAuthenticateFailed = null)
        {
            PlayGamesPlatform.Instance.Authenticate((SignInStatus status) =>
            {
                if (status == SignInStatus.Success)
                {
                    Log($"Successfully signed in with Google Play Games");
                    onAuthenticateSucceeded?.Invoke();
                }
                else
                {
                    PlayGamesPlatform.Instance.ManuallyAuthenticate((SignInStatus status) =>
                    {
                        if (status == SignInStatus.Success)
                        {
                            Log($"Successfully signed in with Google Play Games");
                            onAuthenticateSucceeded?.Invoke();
                        }
                        else
                        {
                            LogError($"Failed to sign in with Google Play Games: {status}");
                            onAuthenticateFailed?.Invoke(ToAuthenticateStatus(status));
                        }
                    });
                }
            });
        }

        public void OpenSaveGame(
            string saveGameName,
            Action<string> onSaveGameOpenedSucceeded = null,
            Action<SaveRequestStatus> onSaveGameOpenedFailed = null)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.OpenWithAutomaticConflictResolution(
                saveGameName,
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseMostRecentlySaved,
                (SavedGameRequestStatus status, ISavedGameMetadata metadata) =>
                {
                    if (status == SavedGameRequestStatus.Success)
                    {
                        Log($"Open Cloud Save Game {metadata.Filename} succeeded");
                        onSaveGameOpenedSucceeded?.Invoke(metadata.Filename);
                    }
                    else
                    {
                        LogError($"Open Cloud Save Game {saveGameName} failed: {status}");
                        onSaveGameOpenedFailed?.Invoke(ToSaveRequestStatus(status));
                    }
                });
        }

       public void ReadSaveGame(
           string saveGameName,
           Action<string> onSaveGameReadSucceeded = null,
           Action<SaveRequestStatus> onSaveGameReadFailed = null)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            OpenSaveGameInternal(
                saveGameName,
                (game) =>
                {
                    savedGameClient.ReadBinaryData(game, (SavedGameRequestStatus status, byte[] data) =>
                    {
                        if (status == SavedGameRequestStatus.Success)
                        {
                            Log($"Read Cloud Save Game {game.Filename} succeeded");
                            string saveString = Encoding.UTF8.GetString(data);
                            onSaveGameReadSucceeded?.Invoke(saveString);
                        }
                        else
                        {
                            LogError($"Read Cloud Save Game {game.Filename} failed: {status}");
                            onSaveGameReadFailed?.Invoke(ToSaveRequestStatus(status));
                        }
                    });
                },
                (status) =>
                {
                    onSaveGameReadFailed?.Invoke(ToSaveRequestStatus(status));
                });
        }

        public void WriteSaveGame(
            string saveGameName, 
            string saveData, 
            TimeSpan totalPlaytime,
            Action onSaveGameSucceeded = null,
            Action<SaveRequestStatus> onSaveGameFailed = null)
        {
            OpenSaveGameInternal(
                saveGameName,
                (game) =>
                {
                    ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
                    UnityEngine.Debug.Assert(savedGameClient != null, $"Google Play Games saved game client is null!");

                    SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
                    builder = builder
                        .WithUpdatedPlayedTime(totalPlaytime)
                        .WithUpdatedDescription($"Saved game at {DateTime.Now}");

                    SavedGameMetadataUpdate updatedMetadata = builder.Build();
                    byte[] savedBytes = Encoding.UTF8.GetBytes(saveData);
                    savedGameClient.CommitUpdate(
                        game,
                        updatedMetadata,
                        savedBytes,
                        (status, metadata) =>
                        {
                            if (status == SavedGameRequestStatus.Success)
                            {
                                Log($"Write Cloud Save Game {metadata.Filename} succeeded");
                                onSaveGameSucceeded?.Invoke();
                            }
                            else
                            {
                                LogError($"Write Cloud Save Game {metadata.Filename} failed: {status}");
                                onSaveGameFailed?.Invoke(ToSaveRequestStatus(status));
                            }
                        });
                },
                (status) =>
                {
                    onSaveGameFailed?.Invoke(ToSaveRequestStatus(status));
                });
            
        }

        public void DeleteSaveGame(
            string saveGameName,
            Action onSaveGameDeletedSucceeded = null,
            Action<SaveRequestStatus> onSaveGameDeletedFailed = null)
        {
            OpenSaveGameInternal(
                saveGameName,
                (game) =>
                {
                    DeleteSaveGameInternal(game, onSaveGameDeletedSucceeded);
                },
                (status) =>
                {
                    onSaveGameDeletedFailed?.Invoke(ToSaveRequestStatus(status));
                });
        }

        #endregion

        #region Internal

        private void SelectSaveGame(
            Action<ISavedGameMetadata> onSavedGameSelectedSucceeded = null,
            Action<SelectUIStatus> onSavedGameSelectedFailed = null)
        {
            uint maxNumToDisplay = 5;
            bool allowCreateNew = false;
            bool allowDelete = true;

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.ShowSelectSavedGameUI("Select Saved Game",
                maxNumToDisplay,
                allowCreateNew,
                allowDelete,
                (SelectUIStatus status, ISavedGameMetadata metadata) =>
                {
                    if (status == SelectUIStatus.SavedGameSelected)
                    {
                        Log($"Select Cloud Save Game {metadata.Filename} succeeded");
                        onSavedGameSelectedSucceeded?.Invoke(metadata);
                    }
                    else
                    {
                        LogError($"Select Cloud Save Game was cancelled or failed: {status}");
                        onSavedGameSelectedFailed?.Invoke(status);
                    }
                });
        }

        private void OpenSaveGameInternal(
            string saveGameName,
            Action<ISavedGameMetadata> onSaveGameOpenedSucceeded = null,
            Action<SavedGameRequestStatus> onSaveGameOpenedFailed = null)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            UnityEngine.Debug.Assert(savedGameClient != null, $"Google Play Games saved game client is null!");
            
            savedGameClient.OpenWithAutomaticConflictResolution(
                saveGameName,
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseManual,
                (SavedGameRequestStatus status, ISavedGameMetadata metadata) =>
                {
                    if (status == SavedGameRequestStatus.Success)
                    {
                        Log($"Open Cloud Save Game {metadata.Filename} succeeded");
                        onSaveGameOpenedSucceeded?.Invoke(metadata);
                    }
                    else
                    {
                        LogError($"Open Cloud Save Game {saveGameName} failed: {status}");
                        onSaveGameOpenedFailed?.Invoke(status);
                    }
                });
        }

        private void DeleteSaveGameInternal(
            ISavedGameMetadata game,
            Action onSaveGameDeletedSucceeded = null)
        {
            Log($"Delete Cloud Save Game {game.Filename} succeeded");
            PlayGamesPlatform.Instance.SavedGame.Delete(game);
            onSaveGameDeletedSucceeded?.Invoke();
        }

        #endregion

        #region Utility

        private void Log(string message)
        {
            if (enableLogging)
            {
                UnityEngine.Debug.Log(message, CelesteLog.CloudSave);
            }
        }

        private void LogError(string message)
        {
            if (enableLogging)
            {
                UnityEngine.Debug.LogError(message, CelesteLog.CloudSave);
            }
        }

        private static AuthenticateStatus ToAuthenticateStatus(SignInStatus signInStatus)
        {
            switch (signInStatus)
            {
                case SignInStatus.Success:
                    return AuthenticateStatus.Success;

                case SignInStatus.InternalError:
                    return AuthenticateStatus.InternalError;

                case SignInStatus.Canceled:
                    return AuthenticateStatus.Cancelled;

                default:
                    UnityEngine.Debug.LogAssertion($"Unhandled {nameof(SignInStatus)} {signInStatus}", CelesteLog.CloudSave);
                    return AuthenticateStatus.InternalError;
            }
        }

        private static SaveRequestStatus ToSaveRequestStatus(SavedGameRequestStatus signInStatus)
        {
            switch (signInStatus)
            {
                case SavedGameRequestStatus.Success:
                    return SaveRequestStatus.Success;

                case SavedGameRequestStatus.AuthenticationError:
                    return SaveRequestStatus.AuthenticationError;

                case SavedGameRequestStatus.BadInputError:
                    return SaveRequestStatus.BadInputError;

                case SavedGameRequestStatus.InternalError:
                    return SaveRequestStatus.InternalError;

                case SavedGameRequestStatus.TimeoutError:
                    return SaveRequestStatus.TimeoutError;

                default:
                    UnityEngine.Debug.LogAssertion($"Unhandled {nameof(SavedGameRequestStatus)} {signInStatus}", CelesteLog.CloudSave);
                    return SaveRequestStatus.InternalError;
            }
        }

        #endregion
    }
}
#endif
