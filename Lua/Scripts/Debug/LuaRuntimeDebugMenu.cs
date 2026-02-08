#if USE_LUA
using Celeste.Debug.Menus;
using UnityEngine;

namespace Celeste.Lua.Debug
{
    [CreateAssetMenu(fileName = nameof(LuaRuntimeDebugMenu), menuName = CelesteMenuItemConstants.LUA_MENU_ITEM + "Debug/Lua Runtime Debug Menu", order = CelesteMenuItemConstants.LUA_MENU_ITEM_PRIORITY)]
    public class LuaRuntimeDebugMenu : DebugMenu
    {
        #region Properties and Fields
        
        [SerializeField] private LuaRuntime luaRuntime;
        
        #endregion
        
        protected override void OnDrawMenu()
        {
            GUILayout.Label($"Is Initialized: {luaRuntime.IsInitialized}");
        }
    }
}
#endif
