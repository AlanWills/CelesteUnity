using Celeste.BoardGame.Catalogue;
using Celeste.BoardGame.Components;
using Celeste.Components;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.BoardGame
{
    [CreateAssetMenu(fileName = nameof(BoardGame), menuName = "Celeste/Board Game/Objects/Board Game")]
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
