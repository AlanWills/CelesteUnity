using System;
using System.IO;
using UnityEngine;

namespace CelesteEditor.UI.Tools
{
    [Serializable]
    public struct CreateRecyclableScrollRectClassesArgs
    {
        [Tooltip("The path of the directory to create the script files")]
        public string directoryPath;
        [Tooltip("The name of the type we will use to generate UI classes to interface with the Recyclable Scroll Rect UI")]
        public string typeName;
        [Tooltip("The root namespace the classes will be created in.  '.UI' will be automatically appended to this")]
        public string namespaceName;

        public void SetDefaultValues()
        {
            directoryPath = "Assets/";
        }
    }

    public static class CreateRecyclableScrollRectClasses
    {
        public static void Generate(
            CreateRecyclableScrollRectClassesArgs args, 
            string controllerClassTemplate,
            string dataClassTemplate,
            string uiClassTemplate)
        {
            string namespaceName = args.namespaceName;
            string typeName = args.typeName;
            string assetDatabasePath = Application.dataPath.Replace("/Assets", "");
            string parentDirectoryPath = !string.IsNullOrEmpty(args.directoryPath) ? Path.Combine(assetDatabasePath, args.directoryPath) : assetDatabasePath;
            Directory.CreateDirectory(parentDirectoryPath);

            string capitalisedTypeName = $"{char.ToUpper(typeName[0])}{typeName.Substring(1)}";
            string lowerCaseTypeName = $"{char.ToLower(typeName[0])}{typeName.Substring(1)}";

            // Controller Class
            {
                controllerClassTemplate = Format(controllerClassTemplate, namespaceName, capitalisedTypeName, lowerCaseTypeName);
                string controllerFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}sUIController.cs");
                File.WriteAllText(controllerFilePath, controllerClassTemplate);
            }

            // UIData Class
            {
                dataClassTemplate = Format(dataClassTemplate, namespaceName, capitalisedTypeName, lowerCaseTypeName);
                string uiDataFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}UIData.cs");
                File.WriteAllText(uiDataFilePath, dataClassTemplate);
            }

            // UI Class
            {
                uiClassTemplate = Format(uiClassTemplate, namespaceName, capitalisedTypeName, lowerCaseTypeName);
                string uiFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}UI.cs");
                File.WriteAllText(uiFilePath, uiClassTemplate);
            }
        }

        private static string Format(string inputString, string _namespace, string type, string lowerCaseType)
        {
            return inputString
                    .Replace("{NAMESPACE}", _namespace, StringComparison.Ordinal)
                    .Replace("{TYPE}", type, StringComparison.Ordinal)
                    .Replace("{LOWER_CASE_TYPE}", lowerCaseType, StringComparison.Ordinal);
        }
    }
}
