using Celeste.Objects;
using UnityEngine;

namespace Celeste.Components.Catalogue
{
    public class ComponentCatalogue<T> : DictionaryScriptableObject<string, T> where T : Component
    {
        public ComponentHandle CreateComponent(string typeName, string jsonData)
        {
            var componentHandle = CreateComponent<T>(typeName, jsonData);
            return new ComponentHandle(componentHandle.component, componentHandle.instance);
        }

        public ComponentHandle<K> CreateComponent<K>(string typeName, string jsonData) where K : T
        {
            K component = GetItem(typeName) as K;

            if (component == null)
            {
                Debug.LogAssertion($"Unable to instantiate component with type name {typeName}.");
                return ComponentHandle<K>.NULL;
            }

            ComponentData componentData = component.CreateData();
            ComponentEvents componentEvents = component.CreateEvents();
            var componentHandle = new ComponentHandle<K>(component, componentData, componentEvents);
            
            JsonUtility.FromJsonOverwrite(jsonData, componentData);
            component.Load(componentHandle.instance);

            return componentHandle;
        }
    }

    [CreateAssetMenu(fileName = nameof(ComponentCatalogue), menuName = "Celeste/Components/Component Catalogue")]
    public class ComponentCatalogue : ComponentCatalogue<Component> { }
}
