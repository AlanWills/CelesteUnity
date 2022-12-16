using Celeste.BoardGame.Runtime;
using Celeste.Components.Persistence;
using Celeste.Objects.Attributes.GUI;
using System;
using System.Collections.Generic;

namespace Celeste.BoardGame.Persistence
{
    [Serializable]
    public class BoardGameObjectRuntimeDTO
    {
        public string name;
        [GuidPicker(typeof(BoardGameObject))] public int guid;
        public List<ComponentDTO> components = new List<ComponentDTO>();

        public BoardGameObjectRuntimeDTO(BoardGameObject boardGameObject)
        {
            name = boardGameObject.name;
            guid = boardGameObject.Guid;

            for (int i = 0, n = boardGameObject.NumComponents; i < n; ++i)
            {
                var component = boardGameObject.GetComponent(i);
                components.Add(ComponentDTO.From(component, component.CreateData()));
            }
        }

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
