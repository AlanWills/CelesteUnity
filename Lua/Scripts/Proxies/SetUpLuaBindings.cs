#if USE_LUA
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Celeste.Assets;
using Celeste.Tools;
using UnityEngine;

namespace Celeste.Lua.Proxies
{
    public class SetUpLuaBindings : MonoBehaviour, IHasAssets
    {
        #region Properties and Fields

        [SerializeField] private LuaRuntime luaRuntime;
        [SerializeField] private List<Component> bindings = new();

        [NonSerialized] private bool hasBound;

        #endregion
        
        #region Unity Methods

        private void OnDisable()
        {
            TryUnbind();
        }

        #endregion
        
        #region IHasAssets
        
        public bool ShouldLoadAssets()
        {
            return true;
        }

        public IEnumerator LoadAssets()
        {
            while (!luaRuntime.IsInitialized)
            {
                // Potentially an infinite loop here, dunno if we should exit out after a certain amount of time
                // This is really only intended to deal with the case where you load straight into a scene in the editor
                yield return null;
            }
            
            TryBind();
        }
        
        #endregion

        private void TryBind()
        {
            if (hasBound)
            {
                return;
            }
            
            foreach (Component component in bindings)
            {
                if (component is ILuaBindings binding)
                {
                    binding.Bind(luaRuntime);
                }
                else
                {
                    UnityEngine.Debug.LogAssertion($"Component detected in {nameof(SetUpLuaBindings)} that is not of type {nameof(ILuaBindings)}.");
                }
            }

            hasBound = true;
        }

        private void TryUnbind()
        {
            if (!hasBound)
            {
                return;
            }
            
            foreach (Component component in bindings)
            {
                if (component is ILuaBindings binding)
                {
                    binding.Unbind(luaRuntime);
                }
                else
                {
                    UnityEngine.Debug.LogAssertion($"Component detected in {nameof(SetUpLuaBindings)} that is not of type {nameof(ILuaBindings)}.");
                }
            }
            
            hasBound = false;
        }
        
        public void FindBindingsInScene()
        {
            bindings.Clear();

            foreach (GameObject root in gameObject.scene.GetRootGameObjects())
            {
                bindings.AddRange(root.GetComponentsInChildren<ILuaBindings>().Select(x => x as Component));
            }
            
            EditorOnly.SetDirty(this);
        }
    }
}
#endif