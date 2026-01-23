using Celeste.Lua;
using TMPro;
using Lua;
using Lua.Unity;

namespace Celeste.UI.Lua
{
    [LuaObject(kName)]
    public partial class LuaTextMeshProUGUIProxy : ILuaProxy
    {
        #region Properties and Fields

        public string Name => kName;

        [LuaMember("text")]
        public string Text
        {
            get => textComponent.text;
            set => textComponent.text = value;
        }

        [LuaMember("color")]
        public void SetColor(float r, float g, float b, float a)
        {
            textComponent.color = new UnityEngine.Color(r, g, b, a);
        }

        [LuaMember("fontSize")]
        public float FontSize
        {
            get => textComponent.fontSize;
            set => textComponent.fontSize = value;
        }
        
        private readonly TextMeshProUGUI textComponent;

        public const string kName = "TextMeshProUGUI";
        
        #endregion

        public LuaTextMeshProUGUIProxy()
        {
        }

        private LuaTextMeshProUGUIProxy(TextMeshProUGUI component)
        {
            textComponent = component;
        }

        public static void Proxy(LuaTable luaTable, ScriptVariable scriptVariable)
        {
            var proxy = new LuaTextMeshProUGUIProxy(scriptVariable.Value as TextMeshProUGUI);
            var lv = LuaValue.FromObject(proxy);
            UnityEngine.Debug.Log($"Type: {lv.Type}");
            luaTable.SetValue(scriptVariable.Name, lv);
        }
    }
}