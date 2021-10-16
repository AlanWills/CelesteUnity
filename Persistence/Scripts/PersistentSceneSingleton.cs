using Celeste.Log;
using Celeste.Objects;
using Celeste.Tools;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Celeste.Persistence
{
    public abstract class PersistentSceneSingleton<T, TDTO> : SceneSingleton<T> 
        where T : PersistentSceneSingleton<T, TDTO>
        where TDTO : class
    {
        #region Properties and Fields

        protected abstract string FileName { get; }

        [SerializeField] private bool loadOnAwake = true;

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            if (loadOnAwake)
            {
                Load();
            }
        }

        #endregion

        #region Load Methods

        public void Load()
        {
            string persistentFilePath = Path.Combine(Application.persistentDataPath, FileName);

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
                            Instance.Deserialize(tDTO);
                        }
                        else
                        {
                            Debug.Log($"Error deserializing data in {persistentFilePath}.  Using default values.");
                            Instance.SetDefaultValues();
                        }
                    }
                    else
                    {
                        Debug.Log($"No data saved to persistent file for {persistentFilePath}.  Using default values.");
                        Instance.SetDefaultValues();
                    }
                }
            }
            else
            {
                Debug.LogFormat($"{persistentFilePath} not found for {nameof(PersistentSceneSingleton<T, TDTO>)} {Instance.name}.  Using default values.");
                Instance.SetDefaultValues();
            }
        }

        public void Save()
        {
            TDTO serializedInstance = Instance.Serialize();
            string persistentFilePath = Path.Combine(Application.persistentDataPath, FileName);

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
                string debugPersistentFilePath = persistentFilePath + ".debug";
                File.WriteAllText(debugPersistentFilePath, JsonUtility.ToJson(serializedInstance));
            }
#endif

            // Needed to deal with browser async saving
            WebGLUtils.SyncFiles();
            HudLog.LogInfo($"{Instance.name} saved");
        }

        protected virtual void OnSaveStart() { }
        protected virtual void OnSaveFinish() { }

        protected abstract TDTO Serialize();
        protected abstract void Deserialize(TDTO dto);
        protected abstract void SetDefaultValues();

        #endregion
    }
}
