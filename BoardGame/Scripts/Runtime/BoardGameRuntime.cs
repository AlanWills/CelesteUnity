using Celeste.BoardGame.Components;
using Celeste.BoardGame.Persistence;
using Celeste.Components;
using Celeste.DataStructures;
using System.Collections.Generic;
using Celeste.Events;

namespace Celeste.BoardGame.Runtime
{
    public class BoardGameRuntime : ComponentContainerRuntime<BoardGameComponent>
    {
        #region Properties and Fields

        public int NumBoardGameObjectRuntimes => boardGameObjectRuntimes.Count;

        private readonly IIndexableItems<BoardGameObject> boardGameObjectCatalogue;
        private readonly IEvent<BoardGameObjectAddedArgs> boardGameObjectAddedEvent;
        private readonly List<BoardGameObjectRuntime> boardGameObjectRuntimes = new();

        #endregion

        public BoardGameRuntime(
            BoardGame boardGame,
            IIndexableItems<BoardGameObject> catalogue,
            IEvent<BoardGameObjectAddedArgs> boardGameObjectAdded)
        {
            boardGameObjectCatalogue = catalogue;
            boardGameObjectAddedEvent = boardGameObjectAdded;

            InitializeComponents(boardGame);
        }

        public void Shutdown()
        {
            foreach (BoardGameObjectRuntime boardGameObjectRuntime in boardGameObjectRuntimes)
            {
                boardGameObjectRuntime.ComponentDataChanged.RemoveListener(OnBoardGameObjectRuntimeChanged);
                boardGameObjectRuntime.ShutdownComponents();
            }
            
            ShutdownComponents();
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
            BoardGameObject boardGameObject =  boardGameObjectCatalogue.FindItem(x => x.Guid == boardGameObjectDTO.guid);
            if (boardGameObject == null)
            {
                UnityEngine.Debug.LogAssertion($"Could not find board game object with guid {boardGameObjectDTO.guid}.");
                return null;
            }

            BoardGameObjectRuntime boardGameObjectRuntime = new BoardGameObjectRuntime(boardGameObject);
            boardGameObjectRuntime.LoadComponents(boardGameObjectDTO.components.ToLookup());
            boardGameObjectRuntime.ComponentDataChanged.AddListener(OnBoardGameObjectRuntimeChanged);
            boardGameObjectRuntimes.Add(boardGameObjectRuntime);
            boardGameObjectAddedEvent?.Invoke(new BoardGameObjectAddedArgs
            {
                boardGameRuntime = this,
                boardGameObjectRuntime = boardGameObjectRuntime,
            });

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

        public BoardGameRuntimeDTO Serialize()
        {
            return new BoardGameRuntimeDTO(this);
        }

        public void Deserialize(BoardGameRuntimeDTO dto)
        {
            LoadComponents(dto.components);
            
            foreach (BoardGameObjectRuntimeDTO boardGameObjectRuntimeDTO in dto.boardGameObjectRuntimes)
            {
                AddBoardGameObjectRuntime(boardGameObjectRuntimeDTO);
            }
        }

        #region Callbacks

        private void OnBoardGameObjectRuntimeChanged()
        {
            ComponentDataChanged.Invoke();
        }

        #endregion
    }
}
