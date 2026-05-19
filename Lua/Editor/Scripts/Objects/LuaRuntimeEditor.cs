#if USE_LUA
using System;
using Celeste.Lua;
using Celeste.Tools;
using UnityEditor;
using UnityEngine;

namespace Lua.Unity.Editor.Objects
{
    [CustomEditor(typeof(LuaRuntime))]
    public class LuaRuntimeEditor : UnityEditor.Editor
    {
        #region Properties and Fields
        
        [NonSerialized] private LuaScript luaScriptToRun;
        
        #endregion

        public override async void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            using (new GUILayout.HorizontalScope())
            {
                luaScriptToRun =
                    EditorGUILayout.ObjectField("Script", luaScriptToRun, typeof(LuaScript), false) as LuaScript;

                using (new GUIEnabledScope(luaScriptToRun != null))
                {
                    if (GUILayout.Button("Execute"))
                    {
                        await (target as LuaRuntime).ExecuteScriptAsync(luaScriptToRun);
                    }
                }
            }
        }
    }
}
#endif