#if USE_LUA
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Celeste.Assets;
using Celeste.Lua.Catalogues;
using Celeste.Lua.Maths;
using Celeste.Lua.Proxies;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Lua.Managers
{
    public class LuaManager : MonoBehaviour, IHasAssets
    {
        #region Properties and Fields

        [SerializeField] private LuaRuntime luaRuntime;
        [SerializeField] private LuaScriptCatalogue runOnInitializeScripts;
        [SerializeField] private UnityEvent<LuaRuntime> bindProxies;

        #endregion
        
        #region Unity Methods

        private void OnDestroy()
        {
            luaRuntime.Shutdown();
        }

        #endregion

        public bool ShouldLoadAssets()
        {
            return true;
        }

        public IEnumerator LoadAssets()
        {
            ValueTask valueTask = luaRuntime.InitializeAsync(new List<ILuaLibrary>
            {
                GUILayoutLibrary.Instance,
                UIToolkitLibrary.Instance
            }, runOnInitializeScripts.Items);

            while (!valueTask.IsCompleted)
            {
                yield return null;
            }
            
            UnityEngine.Debug.Log("Lua Runtime initialized successfully!", CelesteLog.Lua);
            
            luaRuntime.SetEnvironmentVariable(LuaVector2Int.kLuaName, new LuaVector2Int());
            luaRuntime.BindProxy<GameObject, LuaGameObjectProxy>(LuaGameObjectProxy.Bind);
            bindProxies.Invoke(luaRuntime);
        }
    }
}
#endif
