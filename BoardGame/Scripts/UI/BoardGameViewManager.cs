using Celeste.BoardGame.Interfaces;
using Celeste.BoardGame.Runtime;
using Celeste.Components;
using Celeste.Events;
using Celeste.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.BoardGame.UI
{
    [AddComponentMenu("Celeste/Board Game/Board Game View Manager")]
    public class BoardGameViewManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Transform boardAnchor;

        [NonSerialized] private readonly List<BoardGameObjectView> boardGameObjectViews = new();

        #endregion

        private void AddBoardGameObjectUI(BoardGameObjectInstance boardGameObjectInstance, InterfaceHandle<IBoardGameLocations> locations)
        {
            if (boardGameObjectInstance.TryFindComponent<IBoardGameObjectActor>(out var boardGameObjectActor))
            {
                string currentLocation = boardGameObjectActor.iFace.GetCurrentLocationName(boardGameObjectActor.instance);
                Transform boardGameObjectLocation = locations.IsValid ? locations.iFace.FindLocation(currentLocation) : null;
                UnityEngine.Debug.Assert(boardGameObjectLocation != null, $"Failed to find location {currentLocation}.");

                GameObject gameObject = boardGameObjectActor.iFace.InstantiateActor(boardGameObjectActor.instance, boardGameObjectLocation);
                BoardGameObjectView view = gameObject.GetComponent<BoardGameObjectView>();
                ILayoutContainer container = boardGameObjectLocation.gameObject.GetComponent<ILayoutContainer>();

                if (view != null)
                {
                    view.Hookup(boardGameObjectInstance);
                    boardGameObjectViews.Add(view);
                }

                if (container != null)
                {
                    container.OnChildAdded(gameObject);
                }
            }
        }

        private void MoveBoardGameObjectUI(
            BoardGameObjectInstance boardGameObjectInstance, 
            InterfaceHandle<IBoardGameLocations> locations,
            string oldLocation,
            string newLocation)
        {
            if (boardGameObjectInstance.TryFindComponent<IBoardGameObjectActor>(out var boardGameObjectActor))
            {
                Transform oldBoardGameObjectLocation = locations.IsValid ? locations.iFace.FindLocation(oldLocation) : null;
                UnityEngine.Debug.Assert(oldBoardGameObjectLocation != null, $"Failed to find old location {oldLocation}.");

                Transform newBoardGameObjectLocation = locations.IsValid ? locations.iFace.FindLocation(newLocation) : null;
                UnityEngine.Debug.Assert(newBoardGameObjectLocation != null, $"Failed to find new location {newLocation}.");

                ILayoutContainer oldLocationContainer = oldBoardGameObjectLocation.gameObject.GetComponent<ILayoutContainer>();
                ILayoutContainer newLocationContainer = newBoardGameObjectLocation.gameObject.GetComponent<ILayoutContainer>();
                BoardGameObjectView view = boardGameObjectViews.Find(x => x.BoardGameObjectInstance == boardGameObjectInstance);

                if (view != null)
                {
                    view.transform.SetParent(newBoardGameObjectLocation);
                }

                if (oldLocationContainer != null)
                {
                    oldLocationContainer.OnChildRemoved(view.gameObject);
                }

                if (newLocationContainer != null)
                {
                    newLocationContainer.OnChildAdded(view.gameObject);
                }
            }
        }

        #region Callbacks

        public void OnBoardGameReady(BoardGameReadyArgs args)
        {
            BoardGameInstance boardGameInstance = args.BoardGameInstance;

            if (boardGameInstance.TryFindComponent<IBoardGameActor>(out var boardActor))
            {
                boardActor.iFace.InstantiateActor(boardActor.instance, boardAnchor);
            }

            if (!boardGameInstance.TryFindComponent<IBoardGameLocations>(out var locations))
            {
                UnityEngine.Debug.LogAssertion($"Could not find locations interface on board.  This is almost certainly an error...");
            }

            for (int i = 0, n = boardGameInstance.NumBoardGameObjectRuntimes; i < n; ++i)
            {
                AddBoardGameObjectUI(boardGameInstance.GetBoardGameObjectRuntime(i), locations);
            }
        }

        public void OnBoardGameObjectAdded(BoardGameObjectAddedArgs args)
        {
            if (!args.BoardGameInstance.TryFindComponent<IBoardGameLocations>(out var locations))
            {
                UnityEngine.Debug.LogAssertion($"Could not find locations interface on board.  This is almost certainly an error...");
            }

            AddBoardGameObjectUI(args.BoardGameObjectInstance, locations);
        }

        public void OnBoardGameObjectMoved(BoardGameObjectMovedArgs args)
        {
            if (!args.BoardGameInstance.TryFindComponent<IBoardGameLocations>(out var locations))
            {
                UnityEngine.Debug.LogAssertion($"Could not find locations interface on board.  This is almost certainly an error...");
            }

            MoveBoardGameObjectUI(args.BoardGameObjectInstance, locations, args.oldLocation, args.newLocation);
        }

        public void OnBoardGameRuntimeShutdown(BoardGameShutdownArgs args)
        {
            foreach (var boardGameObjectUIController in boardGameObjectViews)
            {
                boardGameObjectUIController.Shutdown();
            }
        }

        #endregion
    }
}
