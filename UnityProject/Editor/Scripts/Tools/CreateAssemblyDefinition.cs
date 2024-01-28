using Celeste.Scene;
using Celeste.Tools.Attributes.GUI;
using CelesteEditor.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
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
        [Tooltip("The full path of the directory that the assembly's directory will be created in")] public string parentDirectory;
        [Tooltip("The name of the assembly's directory that sub directories and files will be created in")] public string directoryName;
        [Tooltip("The name of the assembly project as it will appear in the code solution in your IDE")] public string assemblyName;
        [Tooltip("If true, a code project for runtime script files will be created")] public bool hasRuntimeAssembly;
        [Tooltip("The dependencies to automatically add to the runtime assembly")] [ShowIf(nameof(hasRuntimeAssembly)), HideInInspector] public List<AssemblyDefinitionAsset> runtimeAssemblyDependencies;
        [Tooltip("If true, a code project for editor script files will be created")] public bool hasEditorAssembly;
        [Tooltip("The dependencies to automatically add to the editor assembly")][ShowIf(nameof(hasEditorAssembly)), HideInInspector] public List<AssemblyDefinitionAsset> editorAssemblyDependencies;
        [Tooltip("If true, a menu item will be generated to allow you to load the appropriate scene set for this assembly")] public bool hasSceneMenuItem;
        [Tooltip("The full path to the scene set asset in the project for this assembly")] [ShowIf(nameof(hasSceneMenuItem))] public string sceneSetPath;
        [Tooltip("The full menu item path to load the scene set for this assembly")] [ShowIf(nameof(hasSceneMenuItem))] public string sceneMenuItemPath;
        [Tooltip("If true, the scene set asset for this assembly will be created at the specified path in the project")] [ShowIf(nameof(hasSceneMenuItem))] public bool createSceneSet;

        public void SetDefaultValues()
        {
            parentDirectory = "Assets/";
            sceneSetPath = "Assets/";
            createSceneSet = true;
            runtimeAssemblyDependencies = new List<AssemblyDefinitionAsset>();
            editorAssemblyDependencies = new List<AssemblyDefinitionAsset>();
        }
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

            if (!string.IsNullOrEmpty(parentDirectory))
            {
                AssetUtility.CreateFolder(parentDirectory);
            }

            if (hasRuntimeAssembly)
            {
                List<string> referencedAssemblies = parameters.runtimeAssemblyDependencies.Count > 0 ? new List<string>(parameters.runtimeAssemblyDependencies.Select(x => x.name)) : new List<string>();
                CreateAssembly(parentDirectory, directoryName, assemblyName, assemblyName, referencedAssemblies);
            }

            if (hasEditorAssembly)
            {
                int indexOfFirstDelimiter = assemblyName.IndexOf('.');
                string editorAssemblyNamespace = indexOfFirstDelimiter >= 0 ? $"{assemblyName.Insert(indexOfFirstDelimiter, "Editor")}" : $"{assemblyName}Editor";
                string assemblyDirectoryPath = !string.IsNullOrEmpty(parentDirectory) ? Path.Combine(parentDirectory, directoryName) : directoryName;

                List<string> referencedAssemblies = new List<string>(parameters.editorAssemblyDependencies.Select(x => x.name));

                if (hasRuntimeAssembly)
                {
                    referencedAssemblies.Add(assemblyName);
                }

                if (hasSceneMenuItem)
                {
                    referencedAssemblies.Add("Celeste.Scene.Editor");
                }

                string editorScriptsDirectory = CreateAssembly(
                    assemblyDirectoryPath,
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
            string directoryPath = !string.IsNullOrEmpty(parentDirectoryPath) ? Path.Combine(parentDirectoryPath, directoryName) : directoryName;
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
