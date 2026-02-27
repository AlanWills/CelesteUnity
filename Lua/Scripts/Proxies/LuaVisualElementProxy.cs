#if USE_LUA
using Lua;
using UnityEngine.UIElements;

namespace Celeste.Lua.Proxies
{
    [LuaObject(kName)]
    public partial class LuaVisualElementProxy
    {
        #region Properties and Fields
        
        public VisualElement VisualElement { get; }
        
        private const string kName = "VisualElement";
        
        #endregion

        public LuaVisualElementProxy(VisualElement ve)
        {
            VisualElement = ve;
        }

        [LuaMember("add")] public void Add(LuaVisualElementProxy element)
        {
            VisualElement.Add(element.VisualElement);
        }

        [LuaMember("applyStyle")]
        public void ApplyStyle(LuaTable style)
        {
            UIToolkitLibrary.ApplyStyle(VisualElement, style);
        }

        [LuaMember("clear")]
        public void Clear()
        {
            VisualElement.Clear();
        }
    }
}
#endif