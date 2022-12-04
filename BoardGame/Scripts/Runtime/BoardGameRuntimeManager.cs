using Celeste.Components;
using Celeste.Persistence;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.BoardGame.Runtime
{
    [Serializable]
    public struct BoardGameRuntimeInitializedArgs
    {
        public BoardGameRuntime boardGameRuntime;
    }

    [Serializable]
    public struct BoardGameRuntimeShutdownArgs
    {
    }

    public class BoardGameRuntimeManager : PersistentSceneManager<BoardGameRuntimeManager, BoardGameRuntimeDTO>
    {
        #region Properties and Fields

        public const string FILE_NAME = "BoardGame.dat";
        protected override string FileName => FILE_NAME;

        [SerializeField] private BoardGame boardGame;

        [Header("Events")]
        [SerializeField] private UnityEvent<BoardGameRuntimeInitializedArgs> onRuntimeInitialized = new UnityEvent<BoardGameRuntimeInitializedArgs>();
        [SerializeField] private UnityEvent<BoardGameRuntimeShutdownArgs> onRuntimeShutdown = new UnityEvent<BoardGameRuntimeShutdownArgs>();

        [NonSerialized] private BoardGameRuntime boardGameRuntime;

        #endregion

        #region Unity Methods

        private void Start()
        {
            boardGameRuntime = new BoardGameRuntime(boardGame);

            Load();

            onRuntimeInitialized.Invoke(new BoardGameRuntimeInitializedArgs()
            {
                boardGameRuntime = boardGameRuntime
            });
        }

        private void OnDisable()
        {
            onRuntimeShutdown.Invoke(new BoardGameRuntimeShutdownArgs()
            {
            });
        }

        #endregion

        #region Save/Load

        protected override BoardGameRuntimeDTO Serialize()
        {
            return new BoardGameRuntimeDTO(boardGameRuntime);
        }

        protected override void Deserialize(BoardGameRuntimeDTO dto)
        {
            boardGameRuntime.LoadComponents(dto.components.ToLookup());

            Dictionary<int, BoardGameObjectRuntimeDTO> dtoLookup = new Dictionary<int, BoardGameObjectRuntimeDTO>();

            foreach (var boardGameObjectRuntimeDTO in dto.boardGameObjectRuntimes)
            {
                dtoLookup.Add(boardGameObjectRuntimeDTO.guid, boardGameObjectRuntimeDTO);
            }

            for (int i = 0, n = boardGameRuntime.NumBoardGameObjects; i < n; i++)
            {
                BoardGameObjectRuntime runtime = boardGameRuntime.GetBoardGameObject(i);

                if (dtoLookup.TryGetValue(runtime.Guid, out BoardGameObjectRuntimeDTO boardGameObjectRuntimeDTO))
                {
                    runtime.LoadComponents(boardGameObjectRuntimeDTO.components.ToLookup());
                }
                else
                {
                    Debug.Log($"Looks like a new board game object ({runtime.Name} - {runtime.Guid}) was added prior to the last save.  Setting default values...");
                    runtime.SetDefaultValues();
                }
            }
        }

        protected override void SetDefaultValues()
        {
            boardGameRuntime.SetDefaultValues();
        }

        #endregion
    }
}
