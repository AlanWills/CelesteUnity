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
            typeName = componentHandle.component.GetType().Name;
            data = componentHandle.instance.data.ToJson();
        }

        public ComponentDTO(ComponentTemplate componentTemplate)
        {
            typeName = componentTemplate.component.GetType().Name;
            data = componentTemplate.componentData.ToJson();
        }
    }
}
