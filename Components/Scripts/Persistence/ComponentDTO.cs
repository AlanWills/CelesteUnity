using System;

namespace Celeste.Components.Persistence
{
    [Serializable]
    public struct ComponentDTO
    {
        public string typeName;
        public string data;

        public ComponentDTO(ComponentHandle componentHandle)
        {
            typeName = componentHandle.component.name;
            data = componentHandle.instance.data.ToJson();
        }

        public ComponentDTO(ComponentTemplate componentTemplate)
        {
            typeName = componentTemplate.component.name;
            data = componentTemplate.componentData.ToJson();
        }
    }
}
