using UnityEditor;

namespace CelesteEditor.DeckBuilding.Cards
{
    [CustomEditor(typeof(Celeste.Components.BaseComponent), true, isFallback = true)]
    public class ComponentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script");

            serializedObject.ApplyModifiedProperties();
        }
    }
}