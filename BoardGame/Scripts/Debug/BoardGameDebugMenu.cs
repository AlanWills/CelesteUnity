using Celeste.BoardGame.Events;
using Celeste.BoardGame.Runtime;
using Celeste.Debug.Menus;
using Celeste.Events;
using Celeste.Tools;
using System;
using UnityEngine;

namespace Celeste.BoardGame.Debug
{
    [CreateAssetMenu(fileName = nameof(BoardGameDebugMenu), menuName = "Celeste/Board Game/Debug/Board Game Debug Menu")]
    public class BoardGameDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private BoardGame boardGame;
        [SerializeField] private int entriesPerPage = 20;

        [Header("Events")]
        [SerializeField] private AddBoardGameObjectEvent addBoardGameObjectEvent;
        [SerializeField] private MoveBoardGameObjectEvent moveBoardGameObjectEvent;

        [NonSerialized] private AddBoardGameObjectArgs addBoardGameObjectArgs;
        [NonSerialized] private MoveBoardGameObjectArgs moveBoardGameObjectArgs;
        [NonSerialized] private int currentPage = 0;
        [NonSerialized] private BoardGameRuntime currentBoardGameRuntime;

        #endregion

        protected override void OnDrawMenu()
        {
            using (new GUILayout.HorizontalScope())
            {
                addBoardGameObjectArgs.boardGameObjectGuid = GUIUtils.IntField(addBoardGameObjectArgs.boardGameObjectGuid);
                addBoardGameObjectArgs.location = GUILayout.TextField(addBoardGameObjectArgs.location);

                if (GUILayout.Button("Add", GUILayout.ExpandWidth(false)))
                {
                    addBoardGameObjectEvent.Invoke(addBoardGameObjectArgs);
                    addBoardGameObjectArgs = new AddBoardGameObjectArgs();
                }
            }

            using (new GUILayout.HorizontalScope())
            {
                moveBoardGameObjectArgs.boardGameObjectRuntimeInstanceId = GUIUtils.IntField(moveBoardGameObjectArgs.boardGameObjectRuntimeInstanceId);
                moveBoardGameObjectArgs.newLocation = GUILayout.TextField(moveBoardGameObjectArgs.newLocation);

                if (GUILayout.Button("Move", GUILayout.ExpandWidth(false)))
                {
                    moveBoardGameObjectEvent.Invoke(moveBoardGameObjectArgs);
                    moveBoardGameObjectArgs = new MoveBoardGameObjectArgs();
                }
            }

            if (currentBoardGameRuntime != null)
            {
                currentPage = GUIUtils.ReadOnlyPaginatedList(
                    currentPage,
                    entriesPerPage,
                    currentBoardGameRuntime.NumBoardGameObjectRuntimes,
                    (index) =>
                    {
                        BoardGameObjectRuntime runtime = currentBoardGameRuntime.GetBoardGameObjectRuntime(index);
                        GUILayout.Label($"{runtime.Name} ({runtime.InstanceId})");
                    });
            }
        }

        #region Callbacks

        public void OnBoardGameRuntimeReady(BoardGameReadyArgs boardGameReadyArgs)
        {
            currentBoardGameRuntime = boardGameReadyArgs.boardGameRuntime;
        }

        #endregion
    }
}
