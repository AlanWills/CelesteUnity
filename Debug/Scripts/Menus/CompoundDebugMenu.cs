﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Debug.Menus
{
    [CreateAssetMenu(fileName = nameof(CompoundDebugMenu), menuName = "Celeste/Debug/Compound Debug Menu")]
    public class CompoundDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private List<DebugMenu> debugMenus = new List<DebugMenu>();

        private int currentDebugMenu = -1;

        #endregion

        protected override void OnDrawMenu()
        {
            if (currentDebugMenu < 0)
            {
                for (int i = 0, n = debugMenus.Count; i < n; i++)
                {
                    if (GUILayout.Button(debugMenus[i].DisplayName))
                    {
                        currentDebugMenu = i;
                        debugMenus[i].Visible = true;
                    }
                }
            }
            else
            {
                if (GUILayout.Button("Close"))
                {
                    debugMenus[currentDebugMenu].Visible = false;
                    currentDebugMenu = -1;
                }

                if (currentDebugMenu >= 0)
                {
                    debugMenus[currentDebugMenu].DrawMenu();
                }
            }
        }
    }
}