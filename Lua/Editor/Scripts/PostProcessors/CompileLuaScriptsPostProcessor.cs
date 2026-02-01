#if USE_LUA
using Celeste.Tools;
using Lua;
using Lua.Unity;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Lua.PostProcessors
{
    public class CompileLuaScriptsPostProcessor : AssetPostprocessor
    {
        public override int GetPostprocessOrder()
        {
            // This should be run last (we need to know the LuaScript assets have been created first)
            return int.MaxValue;
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            LuaState luaState = LuaUtility.CreateUnityLuaState();
            
            foreach (LuaScript luaScript in EditorOnly.FindAssets<LuaScript>())
            {
                if (luaScript.TryCompile(luaState))
                {
                    Debug.Log($"Lua script '{luaScript.name}' compiled successfully!", luaScript);
                }
                else
                {
                    Debug.LogError($"Lua script '{luaScript.name}' has compilation errors!", luaScript);
                }
            }
        }
    }
}
#endif