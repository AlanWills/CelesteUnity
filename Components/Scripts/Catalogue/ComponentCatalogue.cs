using Celeste.Objects;
using UnityEngine;

namespace Celeste.Components.Catalogue
{
    [CreateAssetMenu(fileName = nameof(ComponentCatalogue), menuName = "Celeste/Components/Component Catalogue")]
    public class ComponentCatalogue : DictionaryScriptableObject<string, Component>
    {
        public ComponentHandle CreateComponent(string typeName, string jsonData)
        {
            Component original = GetItem(typeName);
            
            if (original == null)
            {
                Debug.LogAssertion($"Unable to instantiate component with type name {typeName}.");
                return new ComponentHandle();
            }

            Component copy = Instantiate(original);
            ComponentData componentData = copy.CreateData();
            ComponentEvents componentEvents = copy.CreateEvents();

            JsonUtility.FromJsonOverwrite(jsonData, componentData);

            return new ComponentHandle(copy, componentData, componentEvents);
        }
    }
}
