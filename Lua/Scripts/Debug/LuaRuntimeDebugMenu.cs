#if USE_LUA
using System;
using Celeste.Debug.Menus;
using Celeste.Tools;
using UnityEngine;

namespace Celeste.Lua.Debug
{
    [CreateAssetMenu(fileName = nameof(LuaRuntimeDebugMenu), menuName = CelesteMenuItemConstants.LUA_MENU_ITEM + "Debug/Lua Runtime Debug Menu", order = CelesteMenuItemConstants.LUA_MENU_ITEM_PRIORITY)]
    public class LuaRuntimeDebugMenu : DebugMenu
    {
        #region Properties and Fields
        
        [SerializeField] private LuaRuntime luaRuntime;

        [NonSerialized] private string luaScriptText;
        
        #endregion
        
        protected override void OnDrawMenu()
        {
            GUILayout.Label($"Is Initialized: {luaRuntime.IsInitialized}");

            using (new GUIEnabledScope(!string.IsNullOrEmpty(luaScriptText)))
            {
                if (GUILayout.Button("Execute"))
                {
                    luaRuntime.ExecuteStringAsync(luaScriptText).FireAndForget("Lua Runtime Debug Menu");
                }
            }

            luaScriptText = GUILayout.TextArea(luaScriptText, GUILayout.MinHeight(400));
        }
    }
}
#endif
