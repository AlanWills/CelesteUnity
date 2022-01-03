using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Platform.Wizards
{
    public static class CreateAssemblyDefinitionConstants
    {
        public const string EDITOR_CONSTANTS = 
            "namespace {0}" +
            "{" +
            "   public static class {1}EditorConstants" +
            "   {" +
            "       public const string SCENE_SET_PATH = \"YOUR_PATH_HERE\";" +
            "   }" +
            "}";

        public const string MENU_ITEMS =
            "using UnityEditor;" +
            "using static CelesteEditor.Scene.MenuItemUtility;" +
            "\n" +
            "namespace {0}" +
            "{" +
            "   public static class MenuItems" +
            "   {" +
            "       [MenuItem(\"YOUR_MENU_PATH_HERE\")]" +
            "       public static void Load{1}MenuItem()" +
            "       {" +
            "           LoadSceneSetMenuItem({1}EditorConstants.SCENE_SET_PATH);" +
            "       }" +
            "   }" +
            "}";
    }
}