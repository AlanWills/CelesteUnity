using Celeste.Log;
using Celeste.Persistence.Settings;
using Celeste.Persistence.Snapshots;
using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Persistence
{
    public abstract class PersistentSceneManager<TManager, TDTO> : MonoBehaviour, IPersistentSceneManager, IInterestedInSnapshots, ISupportsDataSnapshots, ISupportsFileSnapshots
        where TManager : PersistentSceneManager<TManager, TDTO>
        where TDTO : class  // Need for Odin to pick up AOT formatter for serialization
    {
        private enum SaveState
        {
            None,
            Pending,
            InProgress
        }

        #region Properties and Fields

        string IInterestedInSnapshots.UnpackPath => FileName;
        object ISupportsDataSnapshots.Data => Serialize();
        string ISupportsFileSnapshots.SourceFile => FilePath;

        protected abstract string FileName { get; }
        protected string FilePath => Path.Combine(Application.persistentDataPath, FileName);
        protected SnapshotRecord SnapshotRecord => snapshotRecord;

        [SerializeField] private SnapshotRecord snapshotRecord;
        [SerializeField] protected bool loadOnAwake = true;
        [SerializeField, HideIf(nameof(loadOnAwake))] protected bool loadOnStart = false;

        private SaveState currentSaveState = SaveState.None;
        private bool saveRequested = false;
        private Semaphore loadingLock = new Semaphore();

        #endregion

        #region Unity Methods

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (snapshotRecord == null)
            {
                snapshotRecord = PersistenceEditorSettings.GetOrCreateSettings().snapshotRecord;
            }

            if (loadOnAwake && loadOnStart)
            {
                UnityEngine.Debug.LogAssertion($"{name} has both {nameof(loadOnAwake)} and {nameof(loadOnStart)} set to true!");
                loadOnStart = false;
            }
        }
#endif

        protected virtual void Awake()
        {
            snapshotRecord?.RegisterInterestInSnapshots(this);

            if (loadOnAwake)
            {
                Load();
            }
        }

        protected virtual void Start()
        {
            if (!loadOnAwake && loadOnStart)
            {
                // Make sure to only load on start if load on awake is false, otherwise we're going to have a bad time
                Load();
            }
        }

        protected virtual void OnDestroy()
        {
            snapshotRecord?.DeregisterInterestInSnapshots(this);
        }

        #endregion

        #region Load/Save Methods

        public void Load()
        {
            using (SemaphoreScope loadingInProgress = loadingLock.Lock())
            {
                string persistentFilePath = FilePath;

                if (PersistenceUtility.CanLoad(persistentFilePath))
                {
                    TDTO tDTO = PersistenceUtility.Load<TDTO>(persistentFilePath);

                    if (tDTO != null)
                    {
                        Deserialize(tDTO);
                        UnityEngine.Debug.Log($"{name} loaded.", CelesteLog.Persistence);
                    }
                    else
                    {
                        UnityEngine.Debug.LogError($"Error deserializing data in {persistentFilePath}.  Using default values.", CelesteLog.Persistence.WithContext(this));
                        SetDefaultValues();
                    }
                }
                else
                {
                    UnityEngine.Debug.Log($"{persistentFilePath} not found for {GetType().Name} {name}.  Using default values.", CelesteLog.Persistence);
                    SetDefaultValues();
                }
            }
        }

        public void Save()
        {
            if (loadingLock.Locked)
            {
                UnityEngine.Debug.LogError($"{name} is ignoring a save request as loading is in progress.  " +
                    $"You can use {nameof(DelayedSave)} if you really need to save immediately, but it's likely you're accidentally requesting a save during loading.", CelesteLog.Persistence.WithContext(this));
                return;
            }

            if (currentSaveState == SaveState.InProgress)
            {
                // We've already begun the process of saving, so cannot do so again, but we will mark that we should re-save immediately after we are done saving
                saveRequested = true;
                return;
            }

            if (currentSaveState == SaveState.Pending)
            {
                // Our save hasn't started, but will do so at the end of the frame, so no need to re-request the save
                return;
            }

            // We can begin our save process, so let's do that!
            StartCoroutine(DoSave());
        }

        public void DelayedSave()
        {
            StartCoroutine(DoDelayedSave());
        }

        string IPersistentSceneManager.SerializeToString()
        {
            return JsonUtility.ToJson(Serialize());
        }

        private IEnumerator DoSave()
        {
            currentSaveState = SaveState.Pending;

            // We wait until the end of frame, just to buy a bit more time
            yield return new WaitForEndOfFrame();

            currentSaveState = SaveState.Pending;

            Task saveTask = SaveAsync();

            while (!saveTask.IsCompleted)
            {
                yield return null;
            }

            currentSaveState = SaveState.None;

            if (saveTask.IsCompletedSuccessfully)
            {
                UnityEngine.Debug.Log($"{name} saved successfully.", CelesteLog.Core);
            }
            else
            {
                UnityEngine.Debug.LogError($"{name} saved unsuccessfully: {(saveTask.Exception != null ? saveTask.Exception : "no exception")}", CelesteLog.Persistence.WithContext(this));
            }

            if (saveRequested)
            {
                // We've marked, whilst this save was occurring, that we wanted to save again.  We will do so now.
                // Possible optimisation here of checking if the current save is the same as the last save, but that might be overkill
                saveRequested = false;
                Save();
            }
        }

        private IEnumerator DoDelayedSave()
        {
            while (loadingLock.Locked)
            {
                yield return null;
            }
            
            yield return DoSave();
        }

        private async Task SaveAsync()
        {
            TDTO serializedInstance = Serialize();
            await PersistenceUtility.SaveAsync(FilePath, serializedInstance);
        }

        protected virtual void OnSaveStart() { }
        protected virtual void OnSaveFinish() { }

        protected abstract TDTO Serialize();
        protected abstract void Deserialize(TDTO dto);
        protected abstract void SetDefaultValues();

        #endregion
    }
}
