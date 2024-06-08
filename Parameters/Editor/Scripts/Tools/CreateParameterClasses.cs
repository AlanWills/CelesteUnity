using Celeste.Tools.Attributes.GUI;
using System;
using System.IO;
using UnityEngine;

namespace CelesteEditor.Parameters
{
    [Serializable]
    public struct CreateParameterClassesArgs
    {
        [Tooltip("The path of the directory where the script files will be created relative to the root of the project")] public string directoryPath;
        [Tooltip("The name of the parameter type that will be created.  Appropriate suffixes like 'Parameter' will be automatically added")] public string classTypeName;
        [Tooltip("The full qualified namespace name that the classes will be created in")] public string namespaceName;
        [Tooltip("The name of the type that corresponds to the typed argument of the parameter")] public string parameterTypeName;
        [Tooltip("If true, a value parameter class will be generated")] public bool generateValueParameterClasses;
        [Tooltip("If true, a reference class will be generated")] public bool generateReferenceParameterClasses;
        [Tooltip("The full 'Create' menu item path for the value parameter class")][ShowIf(nameof(generateValueParameterClasses))] public string valueParameterCreateAssetMenuPath;
        [Tooltip("The full 'Create' menu item path for the reference parameter class")][ShowIf(nameof(generateReferenceParameterClasses))] public string referenceParameterCreateAssetMenuPath;

        public void SetDefaultValues()
        {
            directoryPath = "Assets/";
            generateValueParameterClasses = true;
            generateReferenceParameterClasses = true;
        }
    }

    public static class CreateParameterClasses
    {
        #region Wizard Methods

        public static void Generate(CreateParameterClassesArgs args, string valueParameterClassTemplate, string referenceParameterClassTemplate)
        {
            string namespaceName = args.namespaceName;
            string classTypeName = args.classTypeName;
            string parameterTypeName = args.parameterTypeName;
            string projectRootPath = Application.dataPath.Remove(0, "Assets/".Length);
            string parentDirectoryPath = !string.IsNullOrEmpty(args.directoryPath) ? Path.Combine(projectRootPath, args.directoryPath) : projectRootPath;
            Directory.CreateDirectory(parentDirectoryPath);

            if (args.generateValueParameterClasses)
            {
                valueParameterClassTemplate = Format(valueParameterClassTemplate, namespaceName, classTypeName, parameterTypeName, args.valueParameterCreateAssetMenuPath);

                string valueParameterFilePath = Path.Combine(parentDirectoryPath, $"{classTypeName}Value.cs");
                File.WriteAllText(valueParameterFilePath, valueParameterClassTemplate);
            }

            if (args.generateReferenceParameterClasses)
            {
                referenceParameterClassTemplate = Format(referenceParameterClassTemplate, namespaceName, classTypeName, parameterTypeName, args.referenceParameterCreateAssetMenuPath);

                string referenceParameterFilePath = Path.Combine(parentDirectoryPath, $"{classTypeName}Reference.cs");
                File.WriteAllText(referenceParameterFilePath, referenceParameterClassTemplate);
            }
        }

        private static string Format(string inputString, string _namespace, string type, string dataType, string menuPath)
        {
            return inputString
                    .Replace("{NAMESPACE}", _namespace, StringComparison.Ordinal)
                    .Replace("{TYPE}", type, StringComparison.Ordinal)
                    .Replace("{DATA_TYPE}", dataType, StringComparison.Ordinal)
                    .Replace("{MENU_PATH}", menuPath, StringComparison.Ordinal);
        }

        #endregion
    }
}
