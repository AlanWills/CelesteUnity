using Celeste.BoardGame.Components;
using Celeste.BoardGame.Persistence;
using Celeste.Components;
using Celeste.DataStructures;
using System;
using System.Collections.Generic;

namespace Celeste.BoardGame.Runtime
{
    [Serializable]
    public struct BoardGameLoadedArgs
    {
        public BoardGameRuntime boardGameRuntime;
    }

    [Serializable]
    public struct BoardGameShutdownArgs
    {
    }

    public class BoardGameRuntime : ComponentContainerRuntime<BoardGameComponent>
    {
        #region Properties and Fields

        public int NumBoardGameObjects => boardGameObjectRuntimes.Count;

        private BoardGame boardGame;
        private List<BoardGameObjectRuntime> boardGameObjectRuntimes = new List<BoardGameObjectRuntime>();

        #endregion

        public BoardGameRuntime(BoardGame boardGame)
        {
            this.boardGame = boardGame;

            InitializeComponents(boardGame);
        }

        public void Shutdown()
        {
            foreach (BoardGameObjectRuntime boardGameObjectRuntime in boardGameObjectRuntimes)
            {
                boardGameObjectRuntime.ComponentDataChanged.RemoveListener(OnBoardGameObjectRuntimeChanged);
                boardGameObjectRuntime.ShutdownComponents();
            }
        }

        public BoardGameObjectRuntime AddBoardGameObject(BoardGameObject boardGameObject)
        {
            BoardGameObjectRuntime boardGameObjectRuntime = new BoardGameObjectRuntime(boardGameObject);
            boardGameObjectRuntime.ComponentDataChanged.AddListener(OnBoardGameObjectRuntimeChanged);
            boardGameObjectRuntimes.Add(boardGameObjectRuntime);

            return boardGameObjectRuntime;
        }

        public BoardGameObjectRuntime AddBoardGameObject(BoardGameObjectRuntimeDTO boardGameObjectDTO)
        {
            BoardGameObject boardGameObject = boardGame.FindBoardGameObject(boardGameObjectDTO.guid);
            if (boardGameObject == null)
            {
                UnityEngine.Debug.LogAssertion($"Could not find board game object with guid {boardGameObjectDTO.guid}.");
                return null;
            }

            BoardGameObjectRuntime boardGameObjectRuntime = new BoardGameObjectRuntime(boardGameObject);
            boardGameObjectRuntime.LoadComponents(boardGameObjectDTO.components.ToLookup());
            boardGameObjectRuntime.ComponentDataChanged.AddListener(OnBoardGameObjectRuntimeChanged);
            boardGameObjectRuntimes.Add(boardGameObjectRuntime);

            return boardGameObjectRuntime;
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
