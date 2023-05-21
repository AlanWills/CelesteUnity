using Celeste.Tilemaps.Components;
using CelesteEditor.Components;
using UnityEditor;

namespace CelesteEditor.Tilemaps.Drawers
{
    [CustomComponentDataDrawer(typeof(SpriteAtlasTileDataComponent))]
    public class SpriteAtlasTileDataComponentDrawer : ComponentDataDrawer
    {
        private SerializedProperty spriteNameProperty;
        private SerializedProperty atlasProperty;
        private CelesteEditorGUILayout.SpriteAtlasSubSpriteSelection selection = new CelesteEditorGUILayout.SpriteAtlasSubSpriteSelection(0, -1);
        
        private const int ENTRIES_PER_PAGE = 20;
        
        protected override void OnEnable()
        {
            base.OnEnable();

            spriteNameProperty = dataProperty.FindPropertyRelative("spriteName");
            atlasProperty = componentObject.FindProperty("spriteAtlas");

            SerializedObject spriteAtlas = new SerializedObject(atlasProperty.objectReferenceValue);
            SerializedProperty namesProperty = spriteAtlas.FindProperty("m_PackedSpriteNamesToIndex");

            for (int i = 0, n = namesProperty.arraySize; i < n; ++i)
            {
                if (string.CompareOrdinal(namesProperty.GetArrayElementAtIndex(i).stringValue, spriteNameProperty.stringValue) == 0)
                {
                    selection.currentPage = i / ENTRIES_PER_PAGE;
                    selection.selectedIndex = i;
                    break;
                }
            }
        }

        protected override void OnInspectorGUI()
        {
            DrawPropertiesExcluding("spriteName");

            selection = CelesteEditorGUILayout.SpriteAtlasSubSpriteField(
                spriteNameProperty,
                atlasProperty,
                selection,
                ENTRIES_PER_PAGE);
        }
    }
}
