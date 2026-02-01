#if USE_LUA
using Lua;
using UnityEngine;

namespace Celeste.Lua.Proxies
{
    [LuaObject(kName)]
    public partial class LuaGameObjectProxy : ILuaProxy
    {
        #region Properties and Fields

        public string Name => kName;
        
        private readonly GameObject gameObject;
        
        private const string kName = "GameObject";
        
        #endregion

        public LuaGameObjectProxy() { }
        
        private LuaGameObjectProxy(GameObject go)
        {
            gameObject = go;
        }
        
        public static ILuaProxy Bind(Object obj)
        {
            return new LuaGameObjectProxy(obj as GameObject);
        }
    }
}
#endif