using Celeste.Scene;
using Celeste.Tools.Attributes.GUI;
using CelesteEditor.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.UnityProject
{
    [Serializable]
    public struct AsmDef
    {
        public string name;
        public string rootNamespace;
        public string[] references;
        public string[] includePlatforms;
        public string[] excludePlatforms;
        public bool allowUnsafeCode;
        public bool overrideReferences;
        public string[] precompiledReferences;
        public bool autoReferenced;
        public string[] defineConstraints;
        public string[] versionDefines;
        public bool noEngineReferences;
    }

    [Serializable]
    public struct CreateAssembliesParameters
    {
        [Tooltip("The path relative to the Assets/ folder of the project.")]
        public string parentDirectory;
        public string directoryName;
        public string assemblyName;
        public bool hasRuntimeAssembly;
        public bool hasEditorAssembly;
        public bool hasSceneMenuItem;
        [Tooltip("The path relative to the project folder (must include Assets/).")]
        [ShowIf(nameof(hasSceneMenuItem))] public string sceneSetPath;
        [ShowIf(nameof(hasSceneMenuItem))] public string sceneMenuItemPath;
        [ShowIf(nameof(hasSceneMenuItem))] public bool createSceneSet;
    }

    public static class CreateAssemblyDefinition
    {
        #region Properties and Fields

        private const string PLACEHOLDER_SCRIPT_NAME = "PlaceholderScript.cs";

        #endregion

        public static void CreateAssemblies(CreateAssembliesParameters parameters)
        {
            string parentDirectory = parameters.parentDirectory;
            string directoryName = parameters.directoryName;
            string assemblyName = parameters.assemblyName;
            bool hasRuntimeAssembly = parameters.hasRuntimeAssembly;
            bool hasEditorAssembly = parameters.hasEditorAssembly;
            bool hasSceneMenuItem = parameters.hasSceneMenuItem;

            string assetDatabasePath = Application.dataPath;
            string parentDirectoryPath = !string.IsNullOrEmpty(parentDirectory) ? Path.Combine(assetDatabasePath, parentDirectory) : assetDatabasePath;
            AssetUtility.CreateFolder(parentDirectoryPath);

            if (hasRuntimeAssembly)
            {
                CreateAssembly(parentDirectoryPath, directoryName, assemblyName, assemblyName);
            }

            if (hasEditorAssembly)
            {
                int indexOfFirstDelimiter = assemblyName.IndexOf('.');
                string editorAssemblyNamespace = indexOfFirstDelimiter >= 0 ? $"{assemblyName.Insert(indexOfFirstDelimiter, "Editor")}" : $"{assemblyName}Editor";

                List<string> referencedAssemblies = new List<string>();

                if (hasRuntimeAssembly)
                {
                    referencedAssemblies.Add(assemblyName);
                }

                if (hasSceneMenuItem)
                {
                    referencedAssemblies.Add("Celeste.Scene.Editor");
                }

                string editorScriptsDirectory = CreateAssembly(
                    Path.Combine(parentDirectoryPath, directoryName),
                    "Editor",
                    $"{assemblyName}.Editor",
                    editorAssemblyNamespace,
                    referencedAssemblies,
                    new string[] { "Editor" });

                if (hasSceneMenuItem)
                {
                    string collapsedAssemblyName = assemblyName.Replace(".", "");

                    // Create the code file for the menu items
                    {
                        string menuItemsScriptPath = Path.Combine(editorScriptsDirectory, $"{collapsedAssemblyName}MenuItems.cs");
                        string menuItemsScript = string.Format(CreateAssemblyDefinitionConstants.MENU_ITEMS, editorAssemblyNamespace, collapsedAssemblyName);
                        File.WriteAllText(menuItemsScriptPath, menuItemsScript);
                    }

                    // Create the code file for the constants
                    {
                        string editorConstantsScriptPath = Path.Combine(editorScriptsDirectory, $"{collapsedAssemblyName}EditorConstants.cs");
                        string editorConstantsScript = string.Format(CreateAssemblyDefinitionConstants.EDITOR_CONSTANTS, editorAssemblyNamespace, collapsedAssemblyName, parameters.sceneSetPath, parameters.sceneMenuItemPath);
                        File.WriteAllText(editorConstantsScriptPath, editorConstantsScript);
                    }

                    // Create the scene set asset if we want to
                    if (parameters.createSceneSet)
                    {
                        SceneSet sceneSet = ScriptableObject.CreateInstance<SceneSet>();
                        sceneSet.name = $"{directoryName}SceneSet";
                        AssetUtility.CreateAsset(sceneSet, parameters.sceneSetPath);
                    }

                    // We've created actual scripts so we can delete the placeholder script now
                    {
                        string placeholderScriptPath = Path.Combine(editorScriptsDirectory, PLACEHOLDER_SCRIPT_NAME);
                        File.Delete(placeholderScriptPath);
                    }
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static string CreateAssembly(
            string parentDirectoryPath,
            string directoryName,
            string assemblyName,
            string assemblyNamespace,
            IReadOnlyList<string> references = null,
            IReadOnlyList<string> includePlatforms = null)
        {
            string directoryPath = Path.Combine(parentDirectoryPath, directoryName);
            AssetUtility.CreateFolder(Path.Combine(directoryPath, "Scripts"));

            AsmDef assemblyDef = new AsmDef();
            assemblyDef.autoReferenced = true;
            assemblyDef.rootNamespace = assemblyNamespace;
            assemblyDef.name = assemblyName;
            assemblyDef.references = references != null ? references.ToArray() : null;
            assemblyDef.includePlatforms = includePlatforms != null ? includePlatforms.ToArray() : null;

            string scriptsDirectory = Path.Combine(directoryPath, "Scripts");
            File.WriteAllText(Path.Combine(scriptsDirectory, $"{assemblyName}.asmdef"), JsonUtility.ToJson(assemblyDef, true));
            File.WriteAllText(Path.Combine(scriptsDirectory, PLACEHOLDER_SCRIPT_NAME), "");

            return scriptsDirectory;
        }

    }
}
