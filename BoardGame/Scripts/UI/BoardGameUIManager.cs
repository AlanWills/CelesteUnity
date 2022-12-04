using Celeste.BoardGame.Interfaces;
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

        [NonSerialized] private List<BoardGameObjectUIController> boardGameObjectUIControllers = new List<BoardGameObjectUIController>();

        #endregion

        #region Callbacks

        public void OnBoardGameRuntimeInitialized(BoardGameRuntimeInitializedArgs args)
        {
            if (args.boardGameRuntime.TryFindComponent<IBoardGameActor>(out var boardActor))
            {
                boardActor.iFace.InstantiateActor(boardActor.instance, boardAnchor);
            }

            bool hasLocations = args.boardGameRuntime.TryFindComponent<IBoardGameLocations>(out var locations);

            for (int i = 0, n = args.boardGameRuntime.NumBoardGameObjects; i < n; ++i)
            {
                BoardGameObjectRuntime boardGameObjectRuntime = args.boardGameRuntime.GetBoardGameObject(i);

                if (boardGameObjectRuntime.TryFindComponent<IBoardGameObjectActor>(out var boardGameObjectActor))
                {
                    Transform boardGameObjectLocation = hasLocations ? locations.iFace.FindLocation(boardGameObjectActor.iFace.GetCurrentLocationName(boardGameObjectActor.instance)) : null;
                    GameObject gameObject = boardGameObjectActor.iFace.InstantiateActor(boardGameObjectActor.instance, boardGameObjectLocation);
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
