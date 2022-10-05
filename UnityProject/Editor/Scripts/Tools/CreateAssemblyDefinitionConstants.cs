namespace CelesteEditor.UnityProject
{
    public static class CreateAssemblyDefinitionConstants
    {
        public const string EDITOR_CONSTANTS = 
            "namespace {0}\n" +
            "{{\n" +
            "   public static class {1}EditorConstants\n" +
            "   {{\n" +
            "       public const string SCENE_SET_PATH = \"YOUR_PATH_HERE\";\n" +
            "       public const string SCENE_MENU_ITEM = \"YOUR_MENU_PATH_HERE\";\n" +
            "   }}\n" +
            "}}";

        public const string MENU_ITEMS =
            "using UnityEditor;\n" +
            "using static CelesteEditor.Scene.MenuItemUtility;\n" +
            "\n\n" +
            "namespace {0}\n" +
            "{{\n" +
            "   public static class MenuItems\n" +
            "   {{\n" +
            "       [MenuItem({1}EditorConstants.SCENE_MENU_ITEM)]\n" +
            "       public static void Load{1}MenuItem()\n" +
            "       {{\n" +
            "           LoadSceneSetMenuItem({1}EditorConstants.SCENE_SET_PATH);\n" +
            "       }}\n" +
            "   }}\n" +
            "}}";
    }
}