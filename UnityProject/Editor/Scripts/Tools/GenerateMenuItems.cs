using CelesteEditor.Tools;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CelesteEditor.UnityProject
{
    public static class GenerateMenuItems
    {
        public static void Execute()
        {
            string[] celesteMenuItemConstantsFiles = Directory.GetFiles($"{Application.dataPath}/..", "*CelesteMenuItemConstants.cs", SearchOption.AllDirectories);

            if (celesteMenuItemConstantsFiles?.Length != 1)
            {
                Debug.LogError("Failed to find the CelesteMenuItemConstants.cs file in the project.  Generation of menu items will fail!");
                return;
            }

            StringBuilder outputFile = new StringBuilder(2048);
            outputFile.AppendLine("namespace Celeste");
            outputFile.AppendLine("{");
            outputFile.AppendLine("    public static class CelesteMenuItemConstants");
            outputFile.AppendLine("    {");

            var assemblyDefinitions = EditorOnly.FindAssets<AssemblyDefinitionAsset>();
            assemblyDefinitions.RemoveAll(x => !x.name.StartsWith("Celeste.") || x.name.Count(c => c == '.') > 1);
            assemblyDefinitions.Sort((a, b) => string.Compare(a.name, b.name));
            
            List<string> assemblyDefinitionNames = assemblyDefinitions.Select(x => x.name.Remove(0, 8).ToUpperInvariant()).ToList();

            outputFile.AppendLine("        public const string CELESTE_MENU_ITEM_ROOT = \"Celeste/\";");
            for (int i = 0, n = assemblyDefinitions.Count; i < n; ++i)
            {
                // Remove the 'Celeste.' from the start of the assembly name
                outputFile.AppendLine($"        public const string {assemblyDefinitionNames[i]}_MENU_ITEM = CELESTE_MENU_ITEM_ROOT + \"{assemblyDefinitions[i].name.Remove(0, 8)}/\";");
            }

            outputFile.AppendLine();

            outputFile.AppendLine("        public const int CELESTE_MENU_ITEM_PRIORITY = 0;");
            for (int i = 0; i < assemblyDefinitionNames.Count; ++i)
            {
                // Remove the 'Celeste.' from the start of the assembly name
                string previousPriority = i == 0 ? "CELESTE" : assemblyDefinitionNames[i - 1];
                outputFile.AppendLine($"        public const int {assemblyDefinitionNames[i]}_MENU_ITEM_PRIORITY = {previousPriority}_MENU_ITEM_PRIORITY + 1;");
            }


            outputFile.AppendLine("    }");
            outputFile.AppendLine("}");

            Debug.Log($"Writing menu items to {celesteMenuItemConstantsFiles[0]}.");
            File.WriteAllText(celesteMenuItemConstantsFiles[0], outputFile.ToString());
            AssetDatabase.Refresh();
        }
    }
}