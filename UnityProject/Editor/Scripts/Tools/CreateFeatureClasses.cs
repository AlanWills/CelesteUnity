using Celeste.Tools.Attributes.GUI;
using CelesteEditor.Tools;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.UnityProject
{
    [Serializable]
    public struct CreateFeatureClassesParameters
    {
        [Header("Assembly")]
        public string runtimeNamespaceName;
        public string editorNamespaceName;

        [Header("Directories")]
        public string runtimeScriptsDirectory;
        public string editorScriptsDirectory;

        [Header("Object")]
        public bool createObject;
        [ShowIf(nameof(createObject))] public string objectTypeName;
        [ShowIf(nameof(createObject))] public string createObjectMenuPath;

        [Header("Catalogue")]
        public bool createCatalogue;
        [ShowIf(nameof(createCatalogue))] public string catalogueTypeName;
        [ShowIf(nameof(createCatalogue))] public string createCatalogueMenuPath;

        [Header("Record")]
        public bool createRecord;
        [ShowIf(nameof(createRecord))] public string recordTypeName;
        [ShowIf(nameof(createRecord))] public string createRecordMenuPath;

        [Header("Manager")]
        public bool createManager;
        [ShowIf(nameof(createManager))] public string managerTypeName;
        [ShowIf(nameof(createManager))] public string addManagerMenuPath;
        [ShowIf(nameof(createManager))] public bool isManagerPersistent;
        [ShowIf(nameof(isManagerPersistent))] public bool managerDTOTypeName;
    }

    public static class CreateFeatureClasses
    {
        public static void Create(CreateFeatureClassesParameters parameters)
        {
            CreateObject(parameters);
            CreateCatalogue(parameters);
            CreateRecord(parameters);
            CreateManager(parameters);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateObject(CreateFeatureClassesParameters parameters)
        {
            if (parameters.createObject)
            {
                string objectsFolderPath = $"{parameters.runtimeScriptsDirectory}/Objects";
                AssetUtility.CreateFolder(objectsFolderPath);

                string objectScriptPath = $"{objectsFolderPath}/{parameters.objectTypeName}.cs";
                string objectScriptContents = string.Format(
                    CreateFeatureClassesConstants.OBJECT_SCRIPT_CONTENTS, 
                    parameters.runtimeNamespaceName, 
                    parameters.objectTypeName, 
                    parameters.createObjectMenuPath);
                File.WriteAllText(objectScriptPath, objectScriptContents);
            }
        }

        private static void CreateCatalogue(CreateFeatureClassesParameters parameters)
        {
            if (parameters.createCatalogue)
            {
                // Catalogue
                {
                    string catalogueFolderPath = $"{parameters.runtimeScriptsDirectory}/Catalogue";
                    AssetUtility.CreateFolder(catalogueFolderPath);

                    string catalogueScriptPath = $"{catalogueFolderPath}/{parameters.catalogueTypeName}.cs";
                    string catalogueScriptContents = string.Format(
                        CreateFeatureClassesConstants.CATALOGUE_SCRIPT_CONTENTS,
                        parameters.runtimeNamespaceName,
                        parameters.catalogueTypeName,
                        parameters.createCatalogueMenuPath,
                        parameters.objectTypeName);
                    File.WriteAllText(catalogueScriptPath, catalogueScriptContents);
                }

                // Catalogue Editor
                {
                    string catalogueEditorFolderPath = $"{parameters.editorScriptsDirectory}/Catalogue";
                    AssetUtility.CreateFolder(catalogueEditorFolderPath);

                    string catalogueEditorScriptPath = $"{catalogueEditorFolderPath}/{parameters.catalogueTypeName}Editor.cs";
                    string catalogueEditorScriptContents = string.Format(
                        CreateFeatureClassesConstants.CATALOGUE_EDITOR_SCRIPT_CONTENTS,
                        parameters.editorNamespaceName,
                        parameters.catalogueTypeName,
                        parameters.objectTypeName);
                    File.WriteAllText(catalogueEditorScriptPath, catalogueEditorScriptContents);
                }
            }
        }

        private static void CreateRecord(CreateFeatureClassesParameters parameters)
        {
            if (parameters.createRecord)
            {
                string recordFolderPath = $"{parameters.runtimeScriptsDirectory}/Record";
                AssetUtility.CreateFolder(recordFolderPath);

                string recordScriptPath = $"{recordFolderPath}/{parameters.recordTypeName}.cs";
                string recordScriptContents = string.Format(
                    CreateFeatureClassesConstants.OBJECT_SCRIPT_CONTENTS,
                    parameters.runtimeNamespaceName,
                    parameters.recordTypeName,
                    parameters.createRecordMenuPath);
                File.WriteAllText(recordScriptPath, recordScriptContents);
            }
        }

        private static void CreateManager(CreateFeatureClassesParameters parameters)
        {
            if (parameters.createManager)
            {
                // Create Manager
                {
                    string managerFolderPath = $"{parameters.runtimeScriptsDirectory}/Managers";
                    AssetUtility.CreateFolder(managerFolderPath);

                    string managerScriptPath = $"{managerFolderPath}/{parameters.managerTypeName}.cs";
                    string managerScriptContents = parameters.isManagerPersistent ?
                        string.Format(
                            CreateFeatureClassesConstants.PERSISTENT_MANAGER_SCRIPT_CONTENTS,
                            parameters.runtimeNamespaceName,
                            parameters.managerTypeName,
                            parameters.addManagerMenuPath,
                            parameters.managerDTOTypeName) :
                        string.Format(
                            CreateFeatureClassesConstants.NON_PERSISTENT_MANAGER_SCRIPT_CONTENTS,
                            parameters.runtimeNamespaceName,
                            parameters.managerTypeName,
                            parameters.addManagerMenuPath);
                    File.WriteAllText(managerScriptPath, managerScriptContents);
                }

                // Create DTO
                if (parameters.isManagerPersistent)
                {
                    string dtoFolderPath = $"{parameters.runtimeScriptsDirectory}/Persistence";
                    AssetUtility.CreateFolder(dtoFolderPath);

                    string dtoScriptPath = $"{dtoFolderPath}/{parameters.managerDTOTypeName}.cs";
                    string dtoScriptContents = string.Format(
                            CreateFeatureClassesConstants.DTO_SCRIPT_CONTENTS,
                            parameters.runtimeNamespaceName,
                            parameters.managerDTOTypeName);
                    File.WriteAllText(dtoScriptPath, dtoScriptContents);
                }
            }
        }
    }
}
