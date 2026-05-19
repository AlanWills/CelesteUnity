using Celeste.BoardGame.Components;
using Celeste.Components;
using UnityEngine;

namespace Celeste.BoardGame
{
    [CreateAssetMenu(
        fileName = nameof(BoardGame), 
        menuName = CelesteMenuItemConstants.BOARDGAME_MENU_ITEM + "Objects/Board Game",
        order = CelesteMenuItemConstants.BOARDGAME_MENU_ITEM_PRIORITY)]
    public class BoardGame : ComponentContainerUsingSubAssets<BoardGameComponent>
    {
    }
}
