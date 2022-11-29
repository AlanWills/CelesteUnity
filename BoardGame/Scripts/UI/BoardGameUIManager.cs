using Celeste.BoardGame.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.BoardGame.UI
{
    [AddComponentMenu("Celeste/Board Game/UI/Board Game UI Manager")]
    public class BoardGameUIManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Transform boardAnchor;

        [NonSerialized] private GameObject currentBoard;
        [NonSerialized] private List<BoardGameObjectUIController> boardGameObjectUIControllers = new List<BoardGameObjectUIController>();

        #endregion

        #region Callbacks

        public void OnBoardGameRuntimeInitialized(BoardGameRuntimeInitializedArgs args)
        {
            currentBoard = args.boardGame.InstantiateBoard(boardAnchor);

            foreach (BoardGameObjectRuntime boardGameObjectRuntime in args.boardGameObjectRuntimes)
            {
                if (boardGameObjectRuntime.TryFindComponent<IBoardGameObjectActor>(out var actor))
                {
                    GameObject gameObject = actor.iFace.InstantiateActor(args.boardGame, actor.instance);
                    BoardGameObjectUIController uiController = gameObject.GetComponent<BoardGameObjectUIController>();

                    if (uiController != null)
                    {
                        uiController.Hookup(boardGameObjectRuntime);
                        boardGameObjectUIControllers.Add(uiController);
                    }
                }
            }
        }

        public void OnBoardGameRuntimeShutdown(BoardGameRuntimeShutdownArgs args)
        {
            foreach (var boardGameObjectUIController in boardGameObjectUIControllers)
            {
                boardGameObjectUIController.Shutdown();
            }
        }

        #endregion
    }
}
