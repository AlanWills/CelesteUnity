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
        [LabelWidth(200)] public string runtimeNamespaceName;
        [LabelWidth(200)] public string editorNamespaceName;

        [Header("Directories")]
        [LabelWidth(200)] public string runtimeScriptsDirectory;
        [LabelWidth(200)] public string editorScriptsDirectory;

        [Header("Object")]
        [LabelWidth(200)] public bool createObject;
        [LabelWidth(200), ShowIf(nameof(createObject))] public string objectTypeName;
        [LabelWidth(200), ShowIf(nameof(createObject))] public string createObjectMenuPath;

        [Header("Catalogue")]
        [LabelWidth(200)] public bool createCatalogue;
        [LabelWidth(200), ShowIf(nameof(createCatalogue))] public string catalogueTypeName;
        [LabelWidth(200), ShowIf(nameof(createCatalogue))] public string createCatalogueMenuPath;

        [Header("Record")]
        [LabelWidth(200)] public bool createRecord;
        [LabelWidth(200), ShowIf(nameof(createRecord))] public string recordTypeName;
        [LabelWidth(200), ShowIf(nameof(createRecord))] public string createRecordMenuPath;

        [Header("Manager")]
        [LabelWidth(200)] public bool createManager;
        [LabelWidth(200), ShowIf(nameof(createManager))] public string managerTypeName;
        [LabelWidth(200), ShowIf(nameof(createManager))] public string addManagerMenuPath;
        [LabelWidth(200), ShowIf(nameof(createManager))] public bool isManagerPersistent;
        [LabelWidth(200), ShowIf(nameof(isManagerPersistent))] public string managerDTOTypeName;
        [LabelWidth(200), ShowIf(nameof(isManagerPersistent)), Tooltip("The name of the script containing the menu items relating to persistence.  Will have 'MenuItems' appended to the name")] 
        public string persistenceMenuItemsName;
        [LabelWidth(200), ShowIf(nameof(isManagerPersistent)), Tooltip("The full menu path of the menu item responsible for opening the save file from within Unity e.g. ProjectName/Save/Open X Save")] public string openMenuPath;
        [LabelWidth(200), ShowIf(nameof(isManagerPersistent)), Tooltip("The full menu path of the menu item responsible for deleting the save file from within Unity e.g. ProjectName/Save/Delete X Save.")] public string deleteMenuPath;

        public void SetDefault()
        {
            runtimeScriptsDirectory = "Assets/";
            editorScriptsDirectory = "Assets/";
        }
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
                        parameters.objectTypeName,
                        parameters.runtimeNamespaceName);
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
                    CreateFeatureClassesConstants.RECORD_SCRIPT_CONTENTS,
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

                if (parameters.isManagerPersistent)
                {
                    // Create DTO
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

                    // Create Persistence Menu Items
                    {
                        string dtoScriptPath = $"{parameters.editorScriptsDirectory}/{parameters.persistenceMenuItemsName}PersistenceMenuItems.cs";
                        string dtoScriptContents = string.Format(
                                CreateFeatureClassesConstants.PERSISTENCE_MENU_ITEMS_SCRIPT_CONTENTS,
                                parameters.editorNamespaceName,
                                parameters.persistenceMenuItemsName,
                                parameters.openMenuPath,
                                parameters.deleteMenuPath,
                                parameters.managerTypeName,
                                parameters.runtimeNamespaceName);
                        File.WriteAllText(dtoScriptPath, dtoScriptContents);
                    }
                }
            }
        }
    }
}
