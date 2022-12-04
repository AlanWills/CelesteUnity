using System;

namespace Celeste.Components.Persistence
{
    [Serializable]
    public class ComponentDTO
    {
        public string typeName;
        public string data;

        private ComponentDTO(Component component, ComponentData componentData)
        {
            typeName = component.name;
            data = componentData.ToJson();
        }

        public static ComponentDTO From(Component component, ComponentData componentData)
        {
            return new ComponentDTO(component, componentData);
        }

        public static ComponentDTO From(ComponentHandle componentHandle)
        {
            return new ComponentDTO(componentHandle.component, componentHandle.instance.data);
        }

        public static ComponentDTO From<T>(ComponentHandle<T> componentHandle) where T : Component
        {
            return new ComponentDTO(componentHandle.component, componentHandle.instance.data);
        }
    }
}
