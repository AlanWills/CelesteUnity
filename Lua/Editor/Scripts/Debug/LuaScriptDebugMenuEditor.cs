#if USE_LUA
using Celeste.Lua.Debug;
using UnityEditor;

namespace Lua.Unity.Editor.Debug
{
    [CustomEditor(typeof(LuaScriptDebugMenu), editorForChildClasses: true, isFallback = true)]
    public class LuaScriptDebugMenuEditor : UnityEditor.Editor
    {
        // The Normal Debug Menu Editor attempts to draw the debug menu in the Inspector
        // Since we rely on a LuaRuntime being initialized this causes problems so we fall back to a normal editor here
    }
}
#endif