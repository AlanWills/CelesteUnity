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
            InitializeComponents(boardGame);

            for (int i = 0, n = boardGame.NumBoardGameObjects; i < n; ++i)
            {
                BoardGameObject boardGameObject = boardGame.GetBoardGameObject(i);
                BoardGameObjectRuntime boardGameObjectRuntime = new BoardGameObjectRuntime(boardGameObject);
                boardGameObjectRuntime.ComponentDataChanged.AddListener(OnBoardGameObjectRuntimeChanged);
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

        public void Shutdown()
        {
            foreach (BoardGameObjectRuntime boardGameObjectRuntime in boardGameObjectRuntimes)
            {
                boardGameObjectRuntime.ComponentDataChanged.RemoveListener(OnBoardGameObjectRuntimeChanged);
                boardGameObjectRuntime.ShutdownComponents();
            }
        }

        public BoardGameObjectRuntime GetBoardGameObject(int index)
        {
            return boardGameObjectRuntimes.Get(index);
        }

        #region Callbacks

        private void OnBoardGameObjectRuntimeChanged()
        {
            ComponentDataChanged.Invoke();
        }

        #endregion
    }
}
