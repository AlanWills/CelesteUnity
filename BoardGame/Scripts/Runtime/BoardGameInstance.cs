using Celeste.BoardGame.Components;
using Celeste.BoardGame.Persistence;
using Celeste.Components;
using Celeste.DataStructures;
using System.Collections.Generic;
using Celeste.Events;

namespace Celeste.BoardGame.Runtime
{
    public class BoardGameInstance : ComponentContainerInstance<BoardGameComponent>
    {
        #region Properties and Fields

        public int NumBoardGameObjectRuntimes => boardGameObjectRuntimes.Count;

        private readonly IIndexableItems<BoardGameObject> boardGameObjectCatalogue;
        private readonly IEvent<BoardGameObjectAddedArgs> boardGameObjectAddedEvent;
        private readonly List<BoardGameObjectInstance> boardGameObjectRuntimes = new();

        #endregion

        public BoardGameInstance(
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
            foreach (BoardGameObjectInstance boardGameObjectRuntime in boardGameObjectRuntimes)
            {
                boardGameObjectRuntime.ComponentDataChanged.RemoveListener(OnBoardGameObjectRuntimeChanged);
                boardGameObjectRuntime.ShutdownComponents();
            }
            
            ShutdownComponents();
        }

        public BoardGameObjectInstance AddBoardGameObjectRuntime(BoardGameObject boardGameObject)
        {
            BoardGameObjectInstance boardGameObjectInstance = new BoardGameObjectInstance(boardGameObject);
            boardGameObjectInstance.ComponentDataChanged.AddListener(OnBoardGameObjectRuntimeChanged);
            boardGameObjectRuntimes.Add(boardGameObjectInstance);

            return boardGameObjectInstance;
        }

        public BoardGameObjectInstance AddBoardGameObjectRuntime(BoardGameObjectRuntimeDTO boardGameObjectDTO)
        {
            BoardGameObject boardGameObject =  boardGameObjectCatalogue.FindItem(x => x.Guid == boardGameObjectDTO.guid);
            if (boardGameObject == null)
            {
                UnityEngine.Debug.LogAssertion($"Could not find board game object with guid {boardGameObjectDTO.guid}.");
                return null;
            }

            BoardGameObjectInstance boardGameObjectInstance = new BoardGameObjectInstance(boardGameObject);
            boardGameObjectInstance.LoadComponents(boardGameObjectDTO.components.ToLookup());
            boardGameObjectInstance.ComponentDataChanged.AddListener(OnBoardGameObjectRuntimeChanged);
            boardGameObjectRuntimes.Add(boardGameObjectInstance);
            boardGameObjectAddedEvent?.Invoke(new BoardGameObjectAddedArgs
            {
                BoardGameInstance = this,
                BoardGameObjectInstance = boardGameObjectInstance,
            });

            return boardGameObjectInstance;
        }

        public BoardGameObjectInstance GetBoardGameObjectRuntime(int index)
        {
            return boardGameObjectRuntimes.Get(index);
        }

        public BoardGameObjectInstance FindBoardGameObjectRuntime(int instanceId)
        {
            return boardGameObjectRuntimes.Find(x => x.InstanceId == instanceId);
        }

        public BoardGameObjectInstance FindBoardGameObjectRuntime(string name)
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
