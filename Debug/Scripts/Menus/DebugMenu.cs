using Celeste.Tools;
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
            get => visible;
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
                if (menuPriority != value)
                {
                    menuPriority = value;
                    EditorOnly.SetDirty(this);
                }
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
                displayName = name.Replace("DebugMenu", string.Empty);
                EditorOnly.SetDirty(this);
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
            
            try
            {
                OnDrawMenu();
            }
            catch (Exception e)
            {
                // Swallow any exceptions from the Debug Menu as there's no point having everything fall over for debug functionality
                // We still log them out though, so we can see what they are
                UnityEngine.Debug.LogException(e);
            }
        }

        protected virtual void OnShowMenu() { }
        protected virtual void OnDrawMenu() { }
        protected virtual void OnHideMenu() { }

        protected GUILayout.VerticalScope Section(string sectionTitle)
        {
            var verticalScope = new GUILayout.VerticalScope(GUI.skin.box);
            
            // Header
            GUILayout.Label(sectionTitle, CelesteGUIStyles.BoldLabel);

            return verticalScope;
        }
    }
}