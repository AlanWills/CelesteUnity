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

        public BoardGameRuntimeDTO(BoardGame boardGame)
        {
            if (boardGame != null)
            {
                components.Capacity = boardGame.NumComponents;

                for (int i = 0, n = boardGame.NumComponents; i < n; ++i)
                {
                    var component = boardGame.GetComponent(i);
                    components.Add(ComponentDTO.From(component, component.CreateData()));
                }
            }
        }

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
                boardGameObjectRuntimes.Capacity = boardGameRuntime.NumBoardGameObjectRuntimes;
                boardGameObjectRuntimes.Clear();

                for (int i = 0, n = boardGameRuntime.NumBoardGameObjectRuntimes; i < n; ++i)
                {
                    boardGameObjectRuntimes.Add(new BoardGameObjectRuntimeDTO(boardGameRuntime.GetBoardGameObjectRuntime(i)));
                }
            }
        }
    }
}
