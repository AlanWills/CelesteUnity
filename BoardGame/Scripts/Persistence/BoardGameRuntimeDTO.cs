using Celeste.BoardGame.Runtime;
using Celeste.Components.Persistence;
using System;
using System.Collections.Generic;

namespace Celeste.BoardGame.Persistence
{
    [Serializable]
    public class BoardGameRuntimeDTO
    {
        public List<ComponentDTO> components = new List<ComponentDTO>();
        public List<BoardGameObjectRuntimeDTO> boardGameObjectRuntimes = new List<BoardGameObjectRuntimeDTO>();

        public BoardGameRuntimeDTO(BoardGameRuntime boardGameRuntime)
        {
            {
                components.Capacity = boardGameRuntime.NumComponents;

                for (int i = 0, n = boardGameRuntime.NumComponents; i < n; ++i)
                {
                    components.Add(ComponentDTO.From(boardGameRuntime.GetComponent(i)));
                }
            }

            {
                boardGameObjectRuntimes.Capacity = boardGameRuntime.NumBoardGameObjects;
                boardGameObjectRuntimes.Clear();

                for (int i = 0, n = boardGameRuntime.NumBoardGameObjects; i < n; ++i)
                {
                    boardGameObjectRuntimes.Add(new BoardGameObjectRuntimeDTO(boardGameRuntime.GetBoardGameObject(i)));
                }
            }
        }
    }
}
