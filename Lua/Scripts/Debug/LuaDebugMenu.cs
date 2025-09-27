#if USE_LUA
using Celeste.Debug.Menus;
using Luny;
using UnityEngine;

namespace Celeste.Lua.Debug
{
    [CreateAssetMenu(fileName = nameof(LuaDebugMenu), menuName = CelesteMenuItemConstants.LUA_MENU_ITEM + "Debug/Lua Debug Menu", order = CelesteMenuItemConstants.LUA_MENU_ITEM_PRIORITY)]
    public class LuaDebugMenu : DebugMenu
    {
        [SerializeField] private LunyLuaAsset debugScriptAsset;
        
        protected override void OnDrawMenu()
        {
            if (GUILayout.Button("Run Debug Script"))
            {
                LunyLuaScript debugScript = LunyLuaScript.LoadFromFileSystem(debugScriptAsset);
                LunyRuntime.Singleton.Lua.RunScript(debugScript);
            }
        }
    }
}
#endif