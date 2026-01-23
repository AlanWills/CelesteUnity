using Celeste.Lua;
using TMPro;
using UnityEngine;

namespace Celeste.UI.Lua
{
    public class CelesteUILuaBindings : MonoBehaviour
    {
        public void Bind(LuaRuntime luaRuntime)
        {
            luaRuntime.BindProxy<TextMeshProUGUI, LuaTextMeshProUGUIProxy>(LuaTextMeshProUGUIProxy.Proxy);
        }
    }
}