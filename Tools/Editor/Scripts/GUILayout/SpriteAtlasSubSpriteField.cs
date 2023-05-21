using Celeste.Tools;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor
{
    public partial class CelesteEditorGUILayout
    {
        public struct SpriteAtlasSubSpriteSelection
        {
            public int currentPage;
            public int selectedIndex;

            public SpriteAtlasSubSpriteSelection(int currentPage, int selectedIndex)
            {
                this.currentPage = currentPage;
                this.selectedIndex = selectedIndex;
            }
        }

        public static SpriteAtlasSubSpriteSelection SpriteAtlasSubSpriteField(
            SerializedProperty spriteNameProperty, 
            SerializedProperty spriteAtlasProperty,
            SpriteAtlasSubSpriteSelection selection,
            int entriesPerPage)
        {
            SerializedObject spriteAtlas = new SerializedObject(spriteAtlasProperty.objectReferenceValue);
            SerializedProperty namesProperty = spriteAtlas.FindProperty("m_PackedSpriteNamesToIndex");
            SerializedProperty spritesProperty = spriteAtlas.FindProperty("m_PackedSprites");

            EditorGUILayout.Space();

            if (selection.selectedIndex >= 0)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    Sprite sprite = spritesProperty.GetArrayElementAtIndex(selection.selectedIndex).objectReferenceValue as Sprite;
                    Texture2D tex = TextureFromSprite(sprite);

                    EditorGUILayout.PrefixLabel("Selected Sprite:");
                    GUILayout.Box(tex);
                    EditorGUILayout.LabelField(sprite.name);
                }
            }
            else
            {
                EditorGUILayout.LabelField("No Selected Sprite");
            }

            HorizontalLineWithSpace();

            int currentPage = selection.currentPage;
            int currentIndex = selection.selectedIndex;

            currentPage = GUIUtils.ReadOnlyPaginatedList(
                currentPage,
                entriesPerPage,
                namesProperty.arraySize,
                (i) =>
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        Sprite sprite = spritesProperty.GetArrayElementAtIndex(i).objectReferenceValue as Sprite;
                        Texture2D tex = TextureFromSprite(sprite);

                        GUILayout.Box(tex);
                        EditorGUILayout.LabelField(sprite.name);

                        using (new EditorGUI.DisabledScope(string.CompareOrdinal(spriteNameProperty.stringValue, sprite.name) == 0))
                        {
                            if (GUILayout.Button("Select", GUILayout.ExpandWidth(false)))
                            {
                                spriteNameProperty.stringValue = sprite.name;
                                currentIndex = i;
                            }
                        }
                    }
                });

            return new SpriteAtlasSubSpriteSelection(currentPage, currentIndex);
        }

        private static Texture2D TextureFromSprite(Sprite sprite)
        {
            var rect = sprite.rect;
            var tex = new Texture2D((int)rect.width, (int)rect.height);
            var data = sprite.texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
            tex.SetPixels(data);
            tex.Apply(true);

            return tex;
        }
    }
}
