using Celeste.BoardGame.Runtime;
using Celeste.Components.Persistence;
using System;
using System.Collections.Generic;

namespace Celeste.BoardGame.Persistence
{
    [Serializable]
    public class BoardGameObjectRuntimeDTO
    {
        public int guid;
        public List<ComponentDTO> components = new List<ComponentDTO>();

        public BoardGameObjectRuntimeDTO(BoardGameObjectRuntime boardGameObjectRuntime)
        {
            guid = boardGameObjectRuntime.Guid;

            for (int i = 0, n = boardGameObjectRuntime.NumComponents; i < n; ++i)
            {
                var componentHandle = boardGameObjectRuntime.GetComponent(i);
                components.Add(ComponentDTO.From(componentHandle));
            }
        }
    }
}
