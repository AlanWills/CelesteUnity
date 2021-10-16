using Celeste.Assets;
using Celeste.Objects;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Celeste.Objects
{
    public abstract class AddressableSingleton<T> : ScriptableObject where T : AddressableSingleton<T>
    {
        #region Properties and Fields
        
        private static T instance;
        public static T Instance
        {
            get
            {
#if UNITY_EDITOR
                if (instance == null)
                {
                    EditorOnly_Load();
                }
#endif
                if (instance == null)
                {
                    Debug.LogAssertionFormat("{0} is null so creating default instance.  Did you forget to wait for Load()?", nameof(T));
                    instance = CreateInstance<T>();

                }
                return instance;
            }
            private set 
            {
                if (instance != null)
                {
                    Debug.LogAssertion($"Instance {nameof(AddressableSingleton<T>)} already loaded.");
                    instance.OnUnload();
                }

                instance = value;

                if (instance != null)
                {
                    instance.OnLoad();
                }
            }
        }

        #endregion

        protected AddressableSingleton() { }

        #region Load Methods

        #region Editor Only

#if UNITY_EDITOR
        private static void EditorOnly_Load()
        {
            var adPath = typeof(T).GetCustomAttribute<AssetDatabasePathAttribute>();
            if (adPath != null)
            {
                var loadedInstance = AssetDatabase.LoadAssetAtPath<T>(adPath.AssetDatabasePath);
                Debug.Assert(loadedInstance != null, $"Could not load {nameof(AddressableSingleton<T>)} at path {adPath.AssetDatabasePath}.");
                Instance = loadedInstance;
            }
        }
#endif

        #endregion

        protected static AsyncOperationHandleWrapper LoadAsync(string addressablePath)
        {
            AsyncOperationHandleWrapper wrapper = new AsyncOperationHandleWrapper();
            LoadAsync(addressablePath, wrapper);

            return wrapper;
        }

        protected static void LoadAsync(string addressablePath, AsyncOperationHandleWrapper wrapper)
        {
            wrapper.handle = Addressables.LoadAssetAsync<T>(addressablePath);
            wrapper.handle.Completed += Load_Completed;
        }

        private static void Load_Completed(AsyncOperationHandle obj)
        {

            if (obj.IsValid() && obj.Result != null)
            {
                Instance = obj.Result as T;
                Debug.Log($"{nameof(T)} loaded.");
            }
            else
            {
                Debug.LogError($"{nameof(T)} load failed.  IsValid: {obj.IsValid()}");
            }
        }

        protected virtual void OnLoad() { }
        protected virtual void OnUnload() { }

        #endregion

        public static void Unload()
        {
            Instance = null;
        }
    }
}
