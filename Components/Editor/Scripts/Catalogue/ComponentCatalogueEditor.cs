using Celeste.Components;
using Celeste.Components.Catalogue;
using CelesteEditor.Objects;
using UnityEditor;

namespace CelesteEditor.Components.Catalogue
{
    public class ComponentCatalogueEditor<T> : DictionaryScriptableObjectEditor<string, T> where T : Component
    {
        protected override string GetKey(T item)
        {
            return item.name;
        }

        protected override void DrawEntry(string key, T value)
        {
            using (var disabled = new EditorGUI.DisabledScope())
            {
                EditorGUILayout.ObjectField(key, value, value.GetType(), false);
            }
        }
    }

    [CustomEditor(typeof(ComponentCatalogue))]
    public class ComponentCatalogueEditor : ComponentCatalogueEditor<Component> { }
}
