using System.Collections.Generic;
using Celeste.Lua.Catalogues;
using Celeste.Lua.Proxies;
using UnityEngine;

namespace Celeste.Lua.Managers
{
    public class LuaManager : MonoBehaviour
    {
#if USE_LUA
        #region Properties and Fields

        [SerializeField] private LuaRuntime luaRuntime;
        [SerializeField] private LuaScriptCatalogue runOnInitializeScripts;

        #endregion
        
        #region Unity Methods

        private void Awake()
        {
            luaRuntime.Initialize(new List<ILuaLibrary>
            {
                GUILayoutLibrary.Instance
            }, runOnInitializeScripts.Items);
            //luaRuntime.BindProxy<GameObject, LuaGameObjectProxy>(LuaGameObjectProxy.Bind);
        }

        private void OnDestroy()
        {
            luaRuntime.Shutdown();
        }

        #endregion
#endif
    }
}