using Celeste.Scene;
using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CelesteEditor.Tools;
using UnityEditor;
using UnityEditor.SceneManagement;
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
    public class CreateAssembliesParameters
    {
        [Tooltip("The full path of the directory that the assembly's directory will be created in")] public string parentDirectory = "Assets/";
        [Tooltip("The name of the assembly's directory that sub directories and files will be created in")] public string directoryName;
        [Tooltip("The name of the assembly project as it will appear in the code solution in your IDE")] public string assemblyName;
        [Tooltip("If true, a code project for runtime script files will be created")] public bool hasRuntimeAssembly;
        [Tooltip("The dependencies to automatically add to the runtime assembly")] [HideInInspector] public List<AssemblyDefinitionAsset> runtimeAssemblyDependencies = new List<AssemblyDefinitionAsset>();
        [Tooltip("If true, a code project for editor script files will be created")] public bool hasEditorAssembly;
        [Tooltip("The dependencies to automatically add to the editor assembly")] [HideInInspector] public List<AssemblyDefinitionAsset> editorAssemblyDependencies = new List<AssemblyDefinitionAsset>();
        [Tooltip("If true, a menu item will be generated to allow you to load the appropriate scene set for this assembly")] public bool createSceneSet;
        [Tooltip("The full path to the scene set asset in the project for this assembly")] [ShowIf(nameof(createSceneSet))] public string sceneSetFolder = "Assets/";
        [Tooltip("The full menu item path to load the scene set for this assembly")] [ShowIf(nameof(createSceneSet))] public string sceneSetMenuItemPath;
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
            bool hasSceneMenuItem = parameters.createSceneSet;

            if (!string.IsNullOrEmpty(parentDirectory))
            {
                EditorOnly.CreateFolder(parentDirectory);
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

                List<string> referencedAssemblies = parameters.editorAssemblyDependencies.Count > 0 ? new List<string>(parameters.editorAssemblyDependencies.Select(x => x.name)) : new List<string>();

                if (hasRuntimeAssembly)
                {
                    referencedAssemblies.Add(assemblyName);
                }

                CreateAssembly(
                    assemblyDirectoryPath,
                    "Editor",
                    $"{assemblyName}.Editor",
                    editorAssemblyNamespace,
                    referencedAssemblies,
                    new[] { "Editor" });
            }

            // Create the scene set asset if we want to
            if (hasSceneMenuItem)
            {
                SceneSet sceneSet = ScriptableObject.CreateInstance<SceneSet>();
                sceneSet.name = $"{directoryName}SceneSet";
                sceneSet.MenuItemPath = parameters.sceneSetMenuItemPath;

#if USE_ADDRESSABLES
                const SceneType sceneType = SceneType.Addressable;
#else
                const SceneType sceneType = SceneType.Baked;
#endif
                sceneSet.AddScene(directoryName, sceneType, false);
                sceneSet.AddScene($"{directoryName}Debug", sceneType, false);
                
                EditorOnly.CreateAsset(sceneSet, $"{parameters.sceneSetFolder}/{sceneSet.name}.asset");

                // Create normal scene
                {
                    UnityEngine.SceneManagement.Scene scene =
                        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                    EditorSceneManager.SaveScene(scene, $"{parameters.sceneSetFolder}/{directoryName}.unity");
                }

                // Create debug scene
                {
                    UnityEngine.SceneManagement.Scene debugScene =
                        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                    EditorSceneManager.SaveScene(debugScene, $"{parameters.sceneSetFolder}/{directoryName}Debug.unity");
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
            EditorOnly.CreateFolder(Path.Combine(directoryPath, "Scripts"));

            AsmDef assemblyDef = new AsmDef();
            assemblyDef.autoReferenced = false;
            assemblyDef.rootNamespace = assemblyNamespace;
            assemblyDef.name = assemblyName;
            assemblyDef.references = references?.ToArray();
            assemblyDef.includePlatforms = includePlatforms?.ToArray();

            string scriptsDirectory = Path.Combine(directoryPath, "Scripts");
            File.WriteAllText(Path.Combine(scriptsDirectory, $"{assemblyName}.asmdef"), JsonUtility.ToJson(assemblyDef, true));
            File.WriteAllText(Path.Combine(scriptsDirectory, PLACEHOLDER_SCRIPT_NAME), "");

            return scriptsDirectory;
        }

    }
}
