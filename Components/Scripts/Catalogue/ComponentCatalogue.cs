using Celeste.Objects;
using UnityEngine;

namespace Celeste.Components.Catalogue
{
    [CreateAssetMenu(fileName = nameof(ComponentCatalogue), menuName = "Celeste/Components/Component Catalogue")]
    public class ComponentCatalogue : DictionaryScriptableObject<string, Component>
    {
        public ComponentHandle CreateComponent(string typeName, string jsonData)
        {
            var componentHandle = CreateComponent<Component>(typeName, jsonData);
            return new ComponentHandle(componentHandle.component, componentHandle.instance);
        }

        public ComponentHandle<T> CreateComponent<T>(string typeName, string jsonData) where T : Component
        {
            T component = GetItem(typeName) as T;

            if (component == null)
            {
                Debug.LogAssertion($"Unable to instantiate component with type name {typeName}.");
                return ComponentHandle<T>.NULL;
            }

            ComponentData componentData = component.CreateData();
            ComponentEvents componentEvents = component.CreateEvents();

            JsonUtility.FromJsonOverwrite(jsonData, componentData);

            return new ComponentHandle<T>(component, componentData, componentEvents);
        }
    }
}
