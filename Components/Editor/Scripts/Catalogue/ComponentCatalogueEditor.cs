using Celeste.Components;
using Celeste.Components.Catalogue;
using CelesteEditor.Objects;
using UnityEditor;

namespace CelesteEditor.Components.Catalogue
{
    [CustomEditor(typeof(ComponentCatalogue))]
    public class ComponentCatalogueEditor : DictionaryScriptableObjectEditor<string, Component>
    {
        protected override string GetKey(Component item)
        {
            return item.name;
        }

        protected override void DrawEntry(string key, Component value)
        {
            using (var disabled = new EditorGUI.DisabledScope())
            {
                EditorGUILayout.ObjectField(key, value, value.GetType(), false);
            }
        }
    }
}
