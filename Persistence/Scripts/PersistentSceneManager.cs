using Celeste.Log;
using Celeste.Tools;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Celeste.Persistence
{
    public abstract class PersistentSceneManager<TManager, TDTO> : MonoBehaviour
        where TManager : PersistentSceneManager<TManager, TDTO>
        where TDTO : class
    {
        #region Properties and Fields

        protected string FilePath
        {
            get { return Path.Combine(Application.persistentDataPath, FileName); }
        }

        protected abstract string FileName { get; }

        [SerializeField] private bool loadOnAwake = true;

        private bool saveRequested = false;

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
            string persistentFilePath = FilePath;

            if (File.Exists(persistentFilePath))
            {
                using (FileStream fileStream = new FileStream(persistentFilePath, FileMode.Open))
                {
                    if (fileStream.Length > 0)
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        TDTO tDTO = bf.Deserialize(fileStream) as TDTO;

                        if (tDTO != null)
                        {
                            Deserialize(tDTO);
                        }
                        else
                        {
                            Debug.Log($"Error deserializing data in {persistentFilePath}.  Using default values.");
                            SetDefaultValues();
                        }
                    }
                    else
                    {
                        Debug.Log($"No data saved to persistent file for {persistentFilePath}.  Using default values.");
                        SetDefaultValues();
                    }
                }
            }
            else
            {
                Debug.LogFormat($"{persistentFilePath} not found for {GetType().Name} {name}.  Using default values.");
                SetDefaultValues();
            }
        }

        public void Save()
        {
            if (!saveRequested)
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
            string persistentFilePath = FilePath;

            // Save binary file
            {
                using (FileStream fileStream = new FileStream(persistentFilePath, FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fileStream, serializedInstance);
                }
            }

#if UNITY_EDITOR
            // Save debug human readable file
            {
                string debugPersistentFilePath = $"{persistentFilePath}.{PersistenceConstants.DEBUG_FILE_EXTENSION}";
                File.WriteAllText(debugPersistentFilePath, JsonUtility.ToJson(serializedInstance, true));
            }
#endif

            // Needed to deal with browser async saving
            WebGLUtils.SyncFiles();
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
