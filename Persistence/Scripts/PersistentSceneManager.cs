using Celeste.Log;
using Celeste.Persistence.Utility;
using System.Collections;
using System.IO;
using UnityEngine;

namespace Celeste.Persistence
{
    public abstract class PersistentSceneManager<TManager, TDTO> : MonoBehaviour
        where TManager : PersistentSceneManager<TManager, TDTO>
        where TDTO : class  // Need for Odin to pick up AOT formatter for serialization
    {
        #region Properties and Fields

        protected string FilePath
        {
            get { return Path.Combine(Application.persistentDataPath, FileName); }
        }

        protected abstract string FileName { get; }

        [SerializeField] private bool loadOnAwake = true;

        private bool saveRequested = false;
        private Semaphore loadingLock = new Semaphore();

        #endregion

        #region Unity Methods

        protected virtual void Awake()
        {
            if (loadOnAwake)
            {
                Load();
            }
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
                        Debug.Log($"Error deserializing data in {persistentFilePath}.  Using default values.");
                        SetDefaultValues();
                    }
                }
                else
                {
                    Debug.LogFormat($"{persistentFilePath} not found for {GetType().Name} {name}.  Using default values.");
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
