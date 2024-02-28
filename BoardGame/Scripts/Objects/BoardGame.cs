using Celeste.BoardGame.Catalogue;
using Celeste.BoardGame.Components;
using Celeste.Components;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.BoardGame
{
    [CreateAssetMenu(
        fileName = nameof(BoardGame), 
        menuName = CelesteMenuItemConstants.BOARDGAME_MENU_ITEM + "Objects/Board Game",
        order = CelesteMenuItemConstants.BOARDGAME_MENU_ITEM_PRIORITY)]
    public class BoardGame : ComponentContainerUsingSubAssets<BoardGameComponent>
    {
        #region Properties and Fields

        [SerializeField] private BoardGameObjectCatalogue boardGameObjectCatalogue;

        #endregion

        public BoardGameObject FindBoardGameObject(int guid)
        {
            return boardGameObjectCatalogue.FindByGuid(guid);
        }
    }
}
