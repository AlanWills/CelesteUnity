﻿using Celeste.BoardGame.Objects;
using Celeste.Debug.Menus;
using UnityEngine;

namespace Celeste.BoardGame.Debug
{
    [CreateAssetMenu(fileName = nameof(DiceDebugMenu), menuName = "Celeste/Board Game/Debug/Dice Debug Menu")]
    public class DiceDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private Dice dice;

        #endregion

        protected override void OnDrawMenu()
        {
            if (GUILayout.Button("Roll All"))
            {
                dice.RollAll();
            }
        }
    }
}