using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Debug.Menus
{
    [AddComponentMenu("Celeste/Debug/Menus/Debug Menus UI")]
    public class DebugMenusUI : MonoBehaviour
    {
        #region Properties and Fields

        private bool visible;
        private bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                inputBlocker.SetActive(visible);
            }
        }

        [SerializeField] private GUISkin guiSkin;
        [SerializeField] private GameObject inputBlocker;

        private List<DebugMenu> debugMenus = new List<DebugMenu>();
        private Vector2 scrollPosition;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            Visible = false;
        }

        private void OnGUI()
        {
            if (!Visible)
            {
                return;
            }

            var oldSkin = GUI.skin;
            GUI.skin = guiSkin;

            using (GUILayout.ScrollViewScope scrollView = new GUILayout.ScrollViewScope(scrollPosition))
            {
                GUILayout.Label("Debug Menu");

                DebugMenu visibleDebugMenu = debugMenus.Find(x => x.Visible);
                
                if (visibleDebugMenu != null)
                {
                    visibleDebugMenu.DrawMenu();
                }
                else
                {
                    foreach (DebugMenu debugMenu in debugMenus)
                    {
                        if (GUILayout.Button(debugMenu.DisplayName))
                        {
                            debugMenu.Visible = true;
                        }
                    }
                }

                scrollPosition = scrollView.scrollPosition;
            }

            GUI.skin = oldSkin;
        }

        #endregion

        #region Callbacks

        public void OnToggle()
        {
            Visible = !Visible;
        }

        public void RegisterDebugMenu(DebugMenu debugMenu)
        {
            if (!debugMenus.Contains(debugMenu))
            {
                debugMenus.Add(debugMenu);
            }
            else
            {
                UnityEngine.Debug.LogAssertion($"Failed to register debug menu.  {debugMenu.name} already exists in {nameof(DebugMenu)}.");
            }
        }

        public void DeregisterDebugMenu(DebugMenu debugMenu)
        {
            if (!debugMenus.Remove(debugMenu))
            {
                UnityEngine.Debug.LogAssertion($"Failed to deregister debug menu.  {debugMenu.name} does not exist in {nameof(DebugMenu)}.");
            }
        }

        #endregion
    }
}