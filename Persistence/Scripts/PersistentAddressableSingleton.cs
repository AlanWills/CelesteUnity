using Celeste.Assets;
using Celeste.Log;
using Celeste.Objects;
using Celeste.Tools;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

namespace Celeste.Persistence
{
    public abstract class PersistentAddressableSingleton<T, TDTO> : AddressableSingleton<T>
        where T : PersistentAddressableSingleton<T, TDTO>
        where TDTO : class
    {
        #region Properties and Fields

        protected abstract string FilePath { get; }

        #endregion

        protected PersistentAddressableSingleton() { }

        #region Save/Load Methods

        protected override void OnLoad()
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
                            Instance.Deserialize(tDTO);
                        }
                        else
                        {
                            Debug.Log($"Error deserializing data in {persistentFilePath}.  Using default manager values.");
                            Instance.SetDefaultValues();
                        }
                    }
                    else
                    {
                        Debug.Log($"No data saved to persistent file for {persistentFilePath}.  Using default manager values.");
                        Instance.SetDefaultValues();
                    }
                }
            }
            else
            {
                Debug.Log($"{persistentFilePath} not found for manager {Instance.name}.  Using default manager values.");
                Instance.SetDefaultValues();
            }
        }

        public void Save()
        {
            // OPTIMIZATION: Batch this?
            using (FileStream fileStream = new FileStream(FilePath, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fileStream, Instance.Serialize());
            }

            // Needed to deal with browser async saving
            WebGLUtils.SyncFiles();
            HudLog.LogInfo($"{Instance.name} saved");
        }

        protected abstract TDTO Serialize();
        protected abstract void Deserialize(TDTO dto);
        protected abstract void SetDefaultValues();

        #endregion

        #region Utility Methods

        public void DeleteSave()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }

        #endregion
    }
}
