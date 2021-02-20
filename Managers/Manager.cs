using Celeste.Assets;
using Celeste.Log;
using Celeste.Tools;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Celeste.Managers
{
    public abstract class Manager<T> : ScriptableObject 
        where T : Manager<T>
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

        protected Manager() { }

        #region Save/Load Methods

        #region Editor Only

#if UNITY_EDITOR

        protected static T EditorOnly_Load(string assetDatabasePath)
        {
            return AssetDatabase.LoadAssetAtPath<T>(assetDatabasePath);
        }

#endif

        #endregion

        protected static AsyncOperationHandleWrapper LoadAsyncImpl(string addressablePath)
        {
            AsyncOperationHandleWrapper wrapper = new AsyncOperationHandleWrapper();
            LoadAsyncImpl(addressablePath, wrapper);
            
            return wrapper;
        }

        protected static void LoadAsyncImpl(string addressablePath, AsyncOperationHandleWrapper wrapper)
        {
            wrapper.handle = Addressables.LoadAssetAsync<T>(addressablePath);
            wrapper.handle.Completed += Load_Completed;
        }

        private static void Load_Completed(AsyncOperationHandle obj)
        {
            Debug.LogFormat("{0} load complete", typeof(T).Name);

            if (obj.IsValid() && obj.Result != null)
            {
                Instance = obj.Result as T;
                HudLog.LogInfoFormat("{0} loaded", Instance.name);
            }
            else
            {
                Debug.LogErrorFormat("{0} load failed.  IsValid: {1}", typeof(T).Name, obj.IsValid());
            }
        }

        #endregion

        public static void Unload()
        {
            instance = null;
        }
    }
}
