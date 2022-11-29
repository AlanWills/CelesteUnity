using Celeste.BoardGame.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.BoardGame.Runtime
{
    [Serializable]
    public struct BoardGameRuntimeInitializedArgs
    {
        public BoardGame boardGame;
        public List<BoardGameObjectRuntime> boardGameObjectRuntimes;
    }

    [Serializable]
    public struct BoardGameRuntimeShutdownArgs
    {
    }

    public class BoardGameRuntimeManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private BoardGame boardGame;

        [Header("Events")]
        [SerializeField] private UnityEvent<BoardGameRuntimeInitializedArgs> onRuntimeInitialized = new UnityEvent<BoardGameRuntimeInitializedArgs>();
        [SerializeField] private UnityEvent<BoardGameRuntimeShutdownArgs> onRuntimeShutdown = new UnityEvent<BoardGameRuntimeShutdownArgs>();

        [NonSerialized] private List<BoardGameObjectRuntime> boardGameObjectRuntimes = new List<BoardGameObjectRuntime>();

        #endregion

        #region Unity Methods

        private void Start()
        {
            for (int i = 0, n = boardGame.NumBoardGameObjects; i < n; ++i)
            {
                BoardGameObject boardGameObject = boardGame.GetBoardGameObject(i);
                BoardGameObjectRuntime boardGameObjectRuntime = new BoardGameObjectRuntime(boardGameObject);
                boardGameObjectRuntimes.Add(boardGameObjectRuntime);
            }

            onRuntimeInitialized.Invoke(new BoardGameRuntimeInitializedArgs()
            {
                boardGame = boardGame,
                boardGameObjectRuntimes = boardGameObjectRuntimes
            });
        }

        private void OnDisable()
        {
            onRuntimeShutdown.Invoke(new BoardGameRuntimeShutdownArgs()
            {
            });
        }

        #endregion
    }
}
