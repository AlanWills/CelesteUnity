using Celeste.Log;
using Celeste.Persistence.Settings;
using Celeste.Persistence.Snapshots;
using Celeste.Persistence.Utility;
using System.Collections;
using System.IO;
using UnityEngine;

namespace Celeste.Persistence
{
    public abstract class PersistentSceneManager<TManager, TDTO> : MonoBehaviour, IInterestedInSnapshots, ISupportsDataSnapshots, ISupportsFileSnapshots
        where TManager : PersistentSceneManager<TManager, TDTO>
        where TDTO : class  // Need for Odin to pick up AOT formatter for serialization
    {
        #region Properties and Fields

        string IInterestedInSnapshots.UnpackPath => FileName;
        object ISupportsDataSnapshots.Data => Serialize();
        string ISupportsFileSnapshots.SourceFile => FilePath;

        protected string FilePath
        {
            get { return Path.Combine(Application.persistentDataPath, FileName); }
        }

        protected abstract string FileName { get; }

        [SerializeField] private SnapshotRecord snapshotRecord;
        [SerializeField] private bool loadOnAwake = true;

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
        }
#endif

        protected virtual void Awake()
        {
            snapshotRecord.RegisterInterestInSnapshots(this);

            if (loadOnAwake)
            {
                Load();
            }
        }

        protected virtual void OnDestroy()
        {
            snapshotRecord.DeregisterInterestInSnapshots(this);
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
                        HudLog.LogInfo($"{name} loaded");
                    }
                    else
                    {
                        UnityEngine.Debug.Log($"Error deserializing data in {persistentFilePath}.  Using default values.");
                        SetDefaultValues();
                    }
                }
                else
                {
                    UnityEngine.Debug.LogFormat($"{persistentFilePath} not found for {GetType().Name} {name}.  Using default values.");
                    SetDefaultValues();
                }
            }
        }

        public void Save()
        {
            if (!loadingLock.Locked && !saveRequested)
            {
                StartCoroutine(DoSave());
            }
        }

        private IEnumerator DoSave()
        {
            yield return new WaitForEndOfFrame();

            SaveImpl();
        }

        private void SaveImpl()
        {
            TDTO serializedInstance = Serialize();
            PersistenceUtility.Save(FilePath, serializedInstance);
            HudLog.LogInfo($"{name} saved");

            saveRequested = false;
        }

        protected virtual void OnSaveStart() { }
        protected virtual void OnSaveFinish() { }

        protected abstract TDTO Serialize();
        protected abstract void Deserialize(TDTO dto);
        protected abstract void SetDefaultValues();

        #endregion
    }
}
