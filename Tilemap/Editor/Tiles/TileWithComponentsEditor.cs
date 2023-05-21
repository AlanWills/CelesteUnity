using Celeste.Tilemaps.Tiles;
using UnityEditor;

namespace CelesteEditor.Tilemaps.Tiles
{
    [CustomEditor(typeof(TileWithComponents))]
    public class TileWithComponentsEditor : Editor
    {
        private Editor componentsEditor;

        private void OnEnable()
        {
            componentsEditor = CreateEditor(serializedObject.FindProperty("components").objectReferenceValue);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script", "components");

            componentsEditor.OnInspectorGUI();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
