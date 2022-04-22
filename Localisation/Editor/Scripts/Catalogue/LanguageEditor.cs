using Celeste.Localisation;
using UnityEditor;

namespace CelesteEditor.Localisation.Catalogue
{
    [CustomEditor(typeof(Language))]
    public class LanguageEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Language language = target as Language;
            EditorGUILayout.LabelField("Num Keys", $"{language.NumLocalisationKeys}");
            EditorGUILayout.LabelField("Num Categories", $"{language.NumLocalisationKeyCategories}");

            base.OnInspectorGUI();
        }
    }
}
