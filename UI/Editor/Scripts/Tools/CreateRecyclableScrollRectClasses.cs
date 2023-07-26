using System;
using System.IO;
using UnityEngine;

namespace CelesteEditor.UI.Tools
{
    [Serializable]
    public struct CreateRecyclableScrollRectClassesArgs
    {
        [Tooltip("The path relative to the Assets/ folder of the project of the directory to create the script files")]
        public string directoryPath;
        [Tooltip("The name of the type we will use to generate UI classes to interface with the Recyclable Scroll Rect UI")]
        public string typeName;
        [Tooltip("The root namespace the classes will be created in.  '.UI' will be automatically appended to this")]
        public string namespaceName;
    }

    public static class CreateRecyclableScrollRectClasses
    {
        public static void Generate(CreateRecyclableScrollRectClassesArgs args)
        {
            string namespaceName = args.namespaceName;
            string typeName = args.typeName;
            string assetDatabasePath = Application.dataPath;
            string parentDirectoryPath = !string.IsNullOrEmpty(args.directoryPath) ? Path.Combine(assetDatabasePath, args.directoryPath) : assetDatabasePath;
            Directory.CreateDirectory(parentDirectoryPath);

            string capitalisedTypeName = $"{char.ToUpper(typeName[0])}{typeName.Substring(1)}";
            string lowerCaseTypeName = $"{char.ToLower(typeName[0])}{typeName.Substring(1)}";

            // Controller Class
            {
                string controllerFileContents = string.Format(CreateRecyclableScrollRectClassesConstants.CONTROLLER_CLASS_FORMAT, namespaceName, capitalisedTypeName, lowerCaseTypeName);
                string controllerFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}sUIController.cs");
                File.WriteAllText(controllerFilePath, controllerFileContents);
            }

            // UIData Class
            {
                string uiDataFileContents = string.Format(CreateRecyclableScrollRectClassesConstants.UI_DATA_CLASS_FORMAT, namespaceName, capitalisedTypeName, lowerCaseTypeName);
                string uiDataFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}UIData.cs");
                File.WriteAllText(uiDataFilePath, uiDataFileContents);
            }

            // UI Class
            {
                string uiFileContents = string.Format(CreateRecyclableScrollRectClassesConstants.UI_CLASS_FORMAT, namespaceName, capitalisedTypeName, lowerCaseTypeName);
                string uiFilePath = Path.Combine(parentDirectoryPath, $"{capitalisedTypeName}UI.cs");
                File.WriteAllText(uiFilePath, uiFileContents);
            }
        }
    }
}
