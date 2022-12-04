using Celeste.BoardGame.Components;
using Celeste.Components;
using Celeste.DataStructures;
using System.Collections.Generic;

namespace Celeste.BoardGame.Runtime
{
    public class BoardGameRuntime : ComponentContainerRuntime<BoardGameComponent>
    {
        #region Properties and Fields
        
        public int NumBoardGameObjects => boardGameObjectRuntimes.Count;

        private List<BoardGameObjectRuntime> boardGameObjectRuntimes = new List<BoardGameObjectRuntime>();

        #endregion

        public BoardGameRuntime(BoardGame boardGame)
        {
            InitComponents(boardGame);

            for (int i = 0, n = boardGame.NumBoardGameObjects; i < n; ++i)
            {
                BoardGameObject boardGameObject = boardGame.GetBoardGameObject(i);
                BoardGameObjectRuntime boardGameObjectRuntime = new BoardGameObjectRuntime(boardGameObject);
                boardGameObjectRuntimes.Add(boardGameObjectRuntime);
            }
        }

        public void SetDefaultValues()
        {
            SetComponentDefaultValues();

            for (int i = 0, n = boardGameObjectRuntimes.Count; i < n; ++i)
            {
                boardGameObjectRuntimes[i].SetDefaultValues();
            }
        }

        public BoardGameObjectRuntime GetBoardGameObject(int index)
        {
            return boardGameObjectRuntimes.Get(index);
        }
    }
}
