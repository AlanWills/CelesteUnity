using Celeste.BoardGame.Components;
using Celeste.BoardGame.Interfaces;
using Celeste.BoardGame.Persistence;
using Celeste.Components;
using Celeste.DataStructures;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.BoardGame.Runtime
{
    public class BoardGameRuntime : ComponentContainerRuntime<BoardGameComponent>
    {
        #region Properties and Fields

        public int NumBoardGameObjectRuntimes => boardGameObjectRuntimes.Count;

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

        public BoardGameObjectRuntime AddBoardGameObjectRuntime(BoardGameObject boardGameObject)
        {
            BoardGameObjectRuntime boardGameObjectRuntime = new BoardGameObjectRuntime(boardGameObject);
            boardGameObjectRuntime.ComponentDataChanged.AddListener(OnBoardGameObjectRuntimeChanged);
            boardGameObjectRuntimes.Add(boardGameObjectRuntime);

            return boardGameObjectRuntime;
        }

        public BoardGameObjectRuntime AddBoardGameObjectRuntime(BoardGameObjectRuntimeDTO boardGameObjectDTO)
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

        public BoardGameObjectRuntime GetBoardGameObjectRuntime(int index)
        {
            return boardGameObjectRuntimes.Get(index);
        }

        public BoardGameObjectRuntime FindBoardGameObjectRuntime(int instanceId)
        {
            return boardGameObjectRuntimes.Find(x => x.InstanceId == instanceId);
        }

        public BoardGameObjectRuntime FindBoardGameObjectRuntime(string name)
        {
            return boardGameObjectRuntimes.Find(x => string.CompareOrdinal(x.Name, name) == 0);
        }

        public void MoveBoardGameObjectRuntime(BoardGameObjectRuntime runtime, string newLocation)
        {
            
        }

        #region Callbacks

        private void OnBoardGameObjectRuntimeChanged()
        {
            ComponentDataChanged.Invoke();
        }

        #endregion
    }
}
