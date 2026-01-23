using Celeste.Lua;
using TMPro;
using Lua;
using UnityEngine;

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
            textComponent.color = new Color(r, g, b, a);
        }

        [LuaMember("fontSize")]
        public float FontSize
        {
            get => textComponent.fontSize;
            set => textComponent.fontSize = value;
        }
        
        private readonly TextMeshProUGUI textComponent;

        private const string kName = "TextMeshProUGUI";
        
        #endregion

        public LuaTextMeshProUGUIProxy()
        {
        }

        private LuaTextMeshProUGUIProxy(TextMeshProUGUI component)
        {
            textComponent = component;
        }

        public static ILuaProxy Proxy(Object obj)
        {
            if (obj is TextMeshProUGUI textMeshPro)
            {
                return new LuaTextMeshProUGUIProxy(textMeshPro);
            }

            return null;
        }
    }
}