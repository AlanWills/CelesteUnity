using Celeste.BoardGame.Interfaces;
using Celeste.BoardGame.Runtime;
using Celeste.Components;
using Celeste.Events;
using Celeste.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

namespace Celeste.BoardGame.UI
{
    [AddComponentMenu("Celeste/Board Game/UI/Board Game UI Manager")]
    public class BoardGameUIManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Transform boardAnchor;

        [NonSerialized] private List<BoardGameObjectUIController> boardGameObjectUIControllers = new List<BoardGameObjectUIController>();

        #endregion

        private void AddBoardGameObjectUI(BoardGameObjectRuntime boardGameObjectRuntime, InterfaceHandle<IBoardGameLocations> locations)
        {
            if (boardGameObjectRuntime.TryFindComponent<IBoardGameObjectActor>(out var boardGameObjectActor))
            {
                string currentLocation = boardGameObjectActor.iFace.GetCurrentLocationName(boardGameObjectActor.instance);
                Transform boardGameObjectLocation = locations.IsValid ? locations.iFace.FindLocation(currentLocation) : null;
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

        #region Callbacks

        public void OnBoardGameReady(BoardGameReadyArgs args)
        {
            BoardGameRuntime boardGameRuntime = args.boardGameRuntime;

            if (boardGameRuntime.TryFindComponent<IBoardGameActor>(out var boardActor))
            {
                boardActor.iFace.InstantiateActor(boardActor.instance, boardAnchor);
            }

            if (!boardGameRuntime.TryFindComponent<IBoardGameLocations>(out var locations))
            {
                UnityEngine.Debug.LogAssertion($"Could not find locations interface on board.  This is almost certainly an error...");
            }

            for (int i = 0, n = boardGameRuntime.NumBoardGameObjects; i < n; ++i)
            {
                AddBoardGameObjectUI(boardGameRuntime.GetBoardGameObject(i), locations);
            }
        }

        public void OnBoardGameObjectAdded(BoardGameObjectAddedArgs args)
        {
            if (!args.boardGameRuntime.TryFindComponent<IBoardGameLocations>(out var locations))
            {
                UnityEngine.Debug.LogAssertion($"Could not find locations interface on board.  This is almost certainly an error...");
            }

            AddBoardGameObjectUI(args.boardGameObjectRuntime, locations);
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
