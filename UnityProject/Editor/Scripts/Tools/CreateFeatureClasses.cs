using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;
using System;
using System.IO;
using System.Web;
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
        public static void Create(
            CreateFeatureClassesParameters parameters,
            string objectClassTemplate,
            string catalogueClassTemplate,
            string catalogueEditorClassTemplate,
            string recordClassTemplate,
            string nonPersistentManagerClassTemplate,
            string persistentManagerClassTemplate,
            string dtoClassTemplate,
            string persistentMenuItemsClassTemplate)
        {
            CreateObject(parameters, objectClassTemplate);
            CreateCatalogue(parameters, catalogueClassTemplate, catalogueEditorClassTemplate);
            CreateRecord(parameters, recordClassTemplate);
            CreateManager(parameters, nonPersistentManagerClassTemplate, persistentManagerClassTemplate, dtoClassTemplate, persistentMenuItemsClassTemplate);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateObject(CreateFeatureClassesParameters parameters, string objectClassTemplate)
        {
            if (parameters.createObject)
            {
                string objectsFolderPath = $"{parameters.runtimeScriptsDirectory}/Objects";
                EditorOnly.CreateFolder(objectsFolderPath);

                string objectScriptPath = $"{objectsFolderPath}/{parameters.objectTypeName}.cs";
                objectClassTemplate = Format(objectClassTemplate, parameters.runtimeNamespaceName, parameters.objectTypeName, parameters.createObjectMenuPath);
                File.WriteAllText(objectScriptPath, objectClassTemplate);
            }
        }

        private static void CreateCatalogue(CreateFeatureClassesParameters parameters, string catalogueClassTemplate, string catalogueEditorClassTemplate)
        {
            if (parameters.createCatalogue)
            {
                // Catalogue
                {
                    string catalogueFolderPath = $"{parameters.runtimeScriptsDirectory}/Catalogue";
                    EditorOnly.CreateFolder(catalogueFolderPath);

                    string catalogueScriptPath = $"{catalogueFolderPath}/{parameters.catalogueTypeName}.cs";
                    catalogueClassTemplate = Format(catalogueClassTemplate, parameters.runtimeNamespaceName, parameters.catalogueTypeName, parameters.createCatalogueMenuPath)
                        .Replace("{OBJECT_TYPE}", parameters.objectTypeName);
                    File.WriteAllText(catalogueScriptPath, catalogueClassTemplate);
                }

                // Catalogue Editor
                {
                    string catalogueEditorFolderPath = $"{parameters.editorScriptsDirectory}/Catalogue";
                    EditorOnly.CreateFolder(catalogueEditorFolderPath);

                    string catalogueEditorScriptPath = $"{catalogueEditorFolderPath}/{parameters.catalogueTypeName}Editor.cs";
                    catalogueEditorClassTemplate = Format(catalogueEditorClassTemplate, parameters.editorNamespaceName, parameters.catalogueTypeName)
                        .Replace("{OBJECT_TYPE}", parameters.objectTypeName)
                        .Replace("{RUNTIME_NAMESPACE}", parameters.runtimeNamespaceName);
                    File.WriteAllText(catalogueEditorScriptPath, catalogueEditorClassTemplate);
                }
            }
        }

        private static void CreateRecord(CreateFeatureClassesParameters parameters, string recordClassTemplate)
        {
            if (parameters.createRecord)
            {
                string recordFolderPath = $"{parameters.runtimeScriptsDirectory}/Record";
                EditorOnly.CreateFolder(recordFolderPath);

                string recordScriptPath = $"{recordFolderPath}/{parameters.recordTypeName}.cs";
                recordClassTemplate = Format(recordClassTemplate, parameters.runtimeNamespaceName, parameters.recordTypeName, parameters.createRecordMenuPath);
                File.WriteAllText(recordScriptPath, recordClassTemplate);
            }
        }

        private static void CreateManager(
            CreateFeatureClassesParameters parameters,
            string nonPersistentManagerClassTemplate,
            string persistentManagerClassTemplate,
            string dtoClassTemplate,
            string persistentMenuItemsClassTemplate)
        {
            if (parameters.createManager)
            {
                // Create Manager
                {
                    string managerFolderPath = $"{parameters.runtimeScriptsDirectory}/Managers";
                    EditorOnly.CreateFolder(managerFolderPath);

                    string managerScriptPath = $"{managerFolderPath}/{parameters.managerTypeName}.cs";
                    string managerScriptContents;

                    if (parameters.isManagerPersistent)
                    {
                        managerScriptContents = Format(persistentManagerClassTemplate, parameters.runtimeNamespaceName, parameters.managerTypeName, parameters.addManagerMenuPath)
                            .Replace("{DTO_TYPE}", parameters.managerDTOTypeName);
                    }
                    else
                    {
                        managerScriptContents = Format(nonPersistentManagerClassTemplate, parameters.runtimeNamespaceName, parameters.managerTypeName, parameters.addManagerMenuPath);
                    }

                    File.WriteAllText(managerScriptPath, managerScriptContents);
                }

                if (parameters.isManagerPersistent)
                {
                    // Create DTO
                    {
                        string dtoFolderPath = $"{parameters.runtimeScriptsDirectory}/Persistence";
                        EditorOnly.CreateFolder(dtoFolderPath);

                        string dtoScriptPath = $"{dtoFolderPath}/{parameters.managerDTOTypeName}.cs";
                        dtoClassTemplate = Format(dtoClassTemplate, parameters.runtimeNamespaceName, parameters.managerDTOTypeName);
                        File.WriteAllText(dtoScriptPath, dtoClassTemplate);
                    }

                    // Create Persistence Menu Items
                    {
                        string dtoScriptPath = $"{parameters.editorScriptsDirectory}/{parameters.persistenceMenuItemsName}PersistenceMenuItems.cs";
                        persistentMenuItemsClassTemplate = Format(persistentMenuItemsClassTemplate, parameters.editorNamespaceName, parameters.persistenceMenuItemsName)
                            .Replace("{OPEN_MENU_PATH}", parameters.openMenuPath)
                            .Replace("{DELETE_MENU_PATH}", parameters.deleteMenuPath)
                            .Replace("{MANAGER_TYPE}", parameters.managerTypeName)
                            .Replace("{RUNTIME_NAMESPACE}", parameters.runtimeNamespaceName);
                        File.WriteAllText(dtoScriptPath, persistentMenuItemsClassTemplate);
                    }
                }
            }
        }

        private static string Format(string inputString, string _namespace, string type)
        {
            return inputString
                    .Replace("{NAMESPACE}", _namespace, StringComparison.Ordinal)
                    .Replace("{TYPE}", type, StringComparison.Ordinal);
        }

        private static string Format(string inputString, string _namespace, string type, string menuPath)
        {
            return Format(inputString, _namespace, type)
                .Replace("{MENU_PATH}", menuPath, StringComparison.Ordinal);
        }
    }
}
