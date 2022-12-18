using Celeste.BoardGame.Interfaces;
using Celeste.BoardGame.Runtime;
using Celeste.UI;
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

        public void OnBoardGameReady(BoardGameRuntime boardGameRuntime)
        {
            if (boardGameRuntime.TryFindComponent<IBoardGameActor>(out var boardActor))
            {
                boardActor.iFace.InstantiateActor(boardActor.instance, boardAnchor);
            }

            bool hasLocations = boardGameRuntime.TryFindComponent<IBoardGameLocations>(out var locations);

            for (int i = 0, n = boardGameRuntime.NumBoardGameObjects; i < n; ++i)
            {
                BoardGameObjectRuntime boardGameObjectRuntime = boardGameRuntime.GetBoardGameObject(i);

                if (boardGameObjectRuntime.TryFindComponent<IBoardGameObjectActor>(out var boardGameObjectActor))
                {
                    string currentLocation = boardGameObjectActor.iFace.GetCurrentLocationName(boardGameObjectActor.instance);
                    Transform boardGameObjectLocation = hasLocations ? locations.iFace.FindLocation(currentLocation) : null;
                    UnityEngine.Debug.Assert(boardGameObjectLocation != null, $"Failed to find location {currentLocation}.");

                    GameObject gameObject = boardGameObjectActor.iFace.InstantiateActor(boardGameObjectActor.instance, boardGameObjectLocation);
                    BoardGameObjectUIController uiController = gameObject.GetComponent<BoardGameObjectUIController>();
                    ILayoutContainer container = boardGameObjectLocation.gameObject.GetComponent<ILayoutContainer>();

                    if (uiController != null)
                    {
                        uiController.Hookup(boardGameObjectRuntime);
                        boardGameObjectUIControllers.Add(uiController);
                    }

                    if (container != null)
                    {
                        container.OnChildAdded(gameObject);
                    }
                }
            }
        }

        public void OnBoardGameRuntimeShutdown(BoardGameShutdownArgs args)
        {
            foreach (var boardGameObjectUIController in boardGameObjectUIControllers)
            {
                boardGameObjectUIController.Shutdown();
            }
        }

        #endregion
    }
}
