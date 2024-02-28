using Celeste.BoardGame.Objects;
using Celeste.Debug.Menus;
using UnityEngine;

namespace Celeste.BoardGame.Debug
{
    [CreateAssetMenu(
        fileName = nameof(DiceDebugMenu), 
        menuName = CelesteMenuItemConstants.BOARDGAME_MENU_ITEM + "Debug/Dice Debug Menu",
        order = CelesteMenuItemConstants.BOARDGAME_MENU_ITEM_PRIORITY)]
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

            if (GUILayout.Button("Reset All Positions"))
            {
                dice.ResetAllPositions();
            }
        }
    }
}
