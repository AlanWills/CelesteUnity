using Celeste.Tools.Attributes.GUI;
using System;
using UnityEngine;

namespace Celeste.Components.Persistence
{
    [Serializable]
    public class ComponentDTO
    {
        public string typeName;
        [Json] public string data;

        private ComponentDTO(BaseComponent component, ComponentData componentData)
        {
            typeName = component.name;
            data = componentData.ToJson();
        }

        public static ComponentDTO From(BaseComponent component, ComponentData componentData)
        {
            return new ComponentDTO(component, componentData);
        }

        public static ComponentDTO From(ComponentHandle componentHandle)
        {
            return new ComponentDTO(componentHandle.component, componentHandle.instance.data);
        }

        public static ComponentDTO From<T>(ComponentHandle<T> componentHandle) where T : BaseComponent
        {
            return new ComponentDTO(componentHandle.component, componentHandle.instance.data);
        }
    }
}
