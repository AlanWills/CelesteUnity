using Celeste.BoardGame.Components;
using Celeste.Components;
using Celeste.DataStructures;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.BoardGame
{
    [CreateAssetMenu(fileName = nameof(BoardGame), menuName = "Celeste/Board Game/Objects/Board Game")]
    public class BoardGame : ComponentContainerUsingSubAssets<BoardGameComponent>
    {
        #region Properties and Fields

        public int NumBoardGameObjects => boardGameObjects.Count;

        [SerializeField] private List<BoardGameObject> boardGameObjects = new List<BoardGameObject>();

        #endregion

        public BoardGameObject GetBoardGameObject(int index)
        {
            return boardGameObjects.Get(index);
        }
    }
}
