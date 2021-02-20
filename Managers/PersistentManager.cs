using Celeste.Assets;
using Celeste.Log;
using Celeste.Managers.DTOs;
using Celeste.Tools;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Celeste.Managers
{
    public abstract class PersistentManager<T, TDTO> : ScriptableObject 
        where T : PersistentManager<T, TDTO>
        where TDTO : class, IPersistentManagerDTO<T, TDTO>
    {
        #region Properties and Fields
        
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.LogAssertionFormat("{0} is null so creating default instance.  Did you forget to wait for Load()", typeof(T).Name);
                    instance = CreateInstance<T>();

                }
                return instance;
            }
            private set { instance = value; }
        }

        #endregion

        protected PersistentManager() { }

        #region Save/Load Methods

        #region Editor Only

#if UNITY_EDITOR

        protected static T EditorOnly_Load(string assetDatabasePath)
        {
            return AssetDatabase.LoadAssetAtPath<T>(assetDatabasePath);
        }

#endif

        #endregion

        public static AsyncOperationHandleWrapper LoadAsyncImpl(string addressablePath, string persistentFilePath)
        {
            AsyncOperationHandleWrapper wrapper = new AsyncOperationHandleWrapper();
            LoadAsyncImpl(addressablePath, persistentFilePath, wrapper);
            
            return wrapper;
        }

        public static void LoadAsyncImpl(string addressablePath, string persistentFilePath, AsyncOperationHandleWrapper wrapper)
        {
            wrapper.handle = Addressables.LoadAssetAsync<T>(addressablePath);
            wrapper.handle.Completed += (AsyncOperationHandle obj) => { Load_Completed(obj, persistentFilePath); };
        }

        private static void Load_Completed(AsyncOperationHandle obj, string persistentFilePath)
        {
            Debug.LogFormat("{0} load complete", typeof(T).Name);

            if (obj.IsValid() && obj.Result != null)
            {
                Instance = obj.Result as T;
                HudLog.LogInfoFormat("{0} loaded", Instance.name);

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
                                Debug.LogFormat("Error deserializing data in {0}.  Using default manager values.", persistentFilePath);
                                Instance.SetDefaultValues();
                            }
                        }
                        else
                        {
                            Debug.LogFormat("No data saved to persistent file for {0}.  Using default manager values.", persistentFilePath);
                            Instance.SetDefaultValues();
                        }
                    }
                }
                else
                {
                    Debug.LogFormat("{0} not found for manager {1}.  Using default manager values.", persistentFilePath, Instance.name);
                    Instance.SetDefaultValues();
                }
            }
            else
            {
                Debug.LogErrorFormat("{0} load failed.  IsValid: {1}", typeof(T).Name, obj.IsValid());
            }
        }

        public void Save(string filePath)
        {
            // OPTIMIZATION: Batch this?
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fileStream, Instance.Serialize());
            }

            // Needed to deal with browser async saving
            WebGLUtils.SyncFiles();
            HudLog.LogInfoFormat("{0} saved", Instance.name);
        }

        protected abstract TDTO Serialize();
        protected abstract void Deserialize(TDTO dto);
        protected abstract void SetDefaultValues();

        #endregion
    }
}
