#if USE_LUA
using Celeste.Lua.Proxies;
using UnityEditor;
using UnityEngine;

namespace LuaEditor.Unity.Proxies
{
    [CustomEditor(typeof(SetUpLuaBindings))]
    public class SetUpLuaBindingsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Find All Bindings In Scene"))
            {
                (target as SetUpLuaBindings).FindBindingsInScene();
            }
            
            base.OnInspectorGUI();
        }
    }
}
#endif