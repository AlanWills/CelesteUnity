using Celeste.BoardGame.Interfaces;
using Celeste.Components;
using Celeste.Events;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.BoardGame.Components
{
    [DisplayName("Tooltip")]
    [CreateAssetMenu(
        fileName = nameof(TooltipBoardGameObjectComponent), 
        menuName = CelesteMenuItemConstants.BOARDGAME_MENU_ITEM + "Board Game Object Components/Tooltip",
        order = CelesteMenuItemConstants.BOARDGAME_MENU_ITEM_PRIORITY)]
    public class TooltipBoardGameObjectComponent : BoardGameObjectComponent, IBoardGameObjectTooltip
    {
        #region Properties and Fields

        [SerializeField] private string tooltip;
        [SerializeField] private ShowTooltipEvent showTooltipEvent;
        [SerializeField] private Celeste.Events.Event hideTooltipEvent;

        #endregion

        public void ShowTooltip(Instance instance, Vector3 position, bool isWorldSpace)
        {
            showTooltipEvent.Invoke(TooltipArgs.AnchoredToMouse(tooltip));
        }

        public void HideTooltip(Instance instance)
        {
            hideTooltipEvent.Invoke();
        }
    }
}
