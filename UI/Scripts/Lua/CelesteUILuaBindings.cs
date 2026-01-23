using Celeste.Lua;
using TMPro;
using UnityEngine;

namespace Celeste.UI.Lua
{
    public class CelesteUILuaBindings : MonoBehaviour
    {
        [SerializeField] private LuaRuntime luaRuntime;

        private void Awake()
        {
            luaRuntime.SetEnvironmentVariable(LuaTextMeshProUGUIProxy.kName, new LuaTextMeshProUGUIProxy());
            luaRuntime.BindProxy<TextMeshProUGUI, LuaTextMeshProUGUIProxy>(LuaTextMeshProUGUIProxy.Proxy);
        }
    }
}