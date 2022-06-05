using Celeste.Objects;
using UnityEngine;

namespace Celeste.Components.Catalogue
{
    [CreateAssetMenu(fileName = nameof(ComponentCatalogue), menuName = "Celeste/Components/Component Catalogue")]
    public class ComponentCatalogue : DictionaryScriptableObject<string, Component>
    {
        public ComponentHandle CreateComponent(string typeName, string jsonData)
        {
            Component component = GetItem(typeName);
            
            if (component == null)
            {
                Debug.LogAssertion($"Unable to instantiate component with type name {typeName}.");
                return ComponentHandle.NULL;
            }

            ComponentData componentData = component.CreateData();
            ComponentEvents componentEvents = component.CreateEvents();

            JsonUtility.FromJsonOverwrite(jsonData, componentData);

            return new ComponentHandle(component, componentData, componentEvents);
        }
    }
}
