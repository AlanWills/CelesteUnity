using UnityEditor;

namespace CelesteEditor.UnityProject
{
    public static class CelesteUnityProjectMenuItems
    {
        [MenuItem("Celeste/Tools/Generate Menu Items")]
        public static void GenerateMenuItemsMenuItem()
        {
            GenerateMenuItems.Execute();
        }

        [MenuItem("Assets/Embed Package", false, 1000000)]
        private static void EmbedPackageMenuItem()
        {
            EmbedPackage.Embed(Selection.activeObject);
        }

        [MenuItem("Assets/Embed Package", true)]
        private static bool EmbedPackageValidation()
        {
            return EmbedPackage.CanEmbed(Selection.activeObject);
        }
    }
}
