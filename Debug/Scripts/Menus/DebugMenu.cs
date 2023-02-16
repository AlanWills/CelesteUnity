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

        public string DisplayName => displayName;

        public int MenuPriority
        {
            get => menuPriority;
            set
            {
                menuPriority = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        [SerializeField] private string displayName;
        [SerializeField, Tooltip("The higher the number, the higher up the debug GUI this will appear.")] private int menuPriority = 0;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(displayName))
            {
                displayName = name;
            }
        }

        #endregion

        public void DrawMenu()
        {
            GUILayout.Space(10);

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label(displayName, GUI.skin.label.New().Bold().UpperCentreAligned());
            }

            OnDrawMenu();
        }

        protected virtual void OnShowMenu() { }
        protected abstract void OnDrawMenu();
        protected virtual void OnHideMenu() { }
    }
}