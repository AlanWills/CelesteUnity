using Celeste.BoardGame.Objects;
using Celeste.Debug.Menus;
using System;
using UnityEngine;

namespace Celeste.BoardGame.Debug
{
    [CreateAssetMenu(fileName = nameof(BoardGameDebugMenu), menuName = "Celeste/Board Game/Debug/Board Game Debug Menu")]
    public class BoardGameDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private BoardGame boardGame;

        [NonSerialized] private int boardGameObjectGuid = 0;

        #endregion

        protected override void OnDrawMenu()
        {
            
        }
    }
}
