using Celeste.Debug.Menus;
using UnityEngine;

namespace Celeste.Input
{
    [CreateAssetMenu(fileName = nameof(InputDebugMenu), menuName = CelesteMenuItemConstants.INPUT_MENU_ITEM + "Debug/Input Debug Menu", order = CelesteMenuItemConstants.INPUT_MENU_ITEM_PRIORITY)]
    public class InputDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private InputState inputState;

        #endregion

        protected override void OnDrawMenu()
        {
        }
    }
}