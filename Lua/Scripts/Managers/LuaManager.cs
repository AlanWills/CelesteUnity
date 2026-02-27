#if USE_LUA
using System.Collections.Generic;
using Celeste.Lua.Catalogues;
using Celeste.Lua.Proxies;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Lua.Managers
{
    public class LuaManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private LuaRuntime luaRuntime;
        [SerializeField] private LuaScriptCatalogue runOnInitializeScripts;
        [SerializeField] private UnityEvent<LuaRuntime> bindProxies;

        #endregion
        
        #region Unity Methods

        private void Awake()
        {
            luaRuntime.Initialize(new List<ILuaLibrary>
            {
                GUILayoutLibrary.Instance,
                UIToolkitLibrary.Instance
            }, runOnInitializeScripts.Items);
            
            luaRuntime.BindProxy<GameObject, LuaGameObjectProxy>(LuaGameObjectProxy.Bind);
            bindProxies.Invoke(luaRuntime);
        }

        private void OnDestroy()
        {
            luaRuntime.Shutdown();
        }

        #endregion
    }
}
#endif
