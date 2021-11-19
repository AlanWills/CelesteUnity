using System;
using UnityEngine;

namespace Celeste.Debug.Menus
{
    public abstract class DebugMenu : ScriptableObject
    {
        #region Properties and Fields

        [NonSerialized] private bool visible;
        public bool Visible
        {
            get { return visible; }
            set 
            {
                if (visible && !value)
                {
                    visible = false;
                    OnHideMenu();
                }
                else if (!visible && value)
                {
                    visible = true;
                    OnShowMenu();
                }
            }
        }

        public string DisplayName
        {
            get { return displayName; }
        }

        [SerializeField] private string displayName;

        #endregion

        public void DrawMenu()
        {
            if (GUILayout.Button("Back"))
            {
                Visible = false;
            }

            GUILayout.Space(10);
            GUILayout.Label(displayName);
            GUILayout.Space(10);

            OnDrawMenu();
        }

        protected virtual void OnShowMenu() { }
        protected abstract void OnDrawMenu();
        protected virtual void OnHideMenu() { }
    }
}