using System;
using System.Collections.Generic;

namespace Celeste.Components.Persistence
{
    [Serializable]
    public class ComponentsDTO
    {
        public List<ComponentDTO> components = new List<ComponentDTO>();
    }
}
