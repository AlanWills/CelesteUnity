using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static CelesteEditor.BuildSystem.Wizards.CreateAssemblyDefinitionConstants;

namespace CelesteEditor.BuildSystem.Wizards
{
    public class CreateAssemblyDefinitionWizard : ScriptableWizard
    {
        [Serializable]
        private struct AsmDef
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

        #region Properties and Fields

        [Tooltip("The path relative to the Assets/ folder of the project.")]
        [SerializeField] private string parentDirectory;
        [SerializeField] private string directoryName;
        [SerializeField] private string assemblyName;
        [SerializeField] private bool hasEditorAssembly = true;
        [SerializeField] private bool hasSceneMenuItem = false;

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Tools/Create Assembly Definition")]
        public static void ShowCreateAssemblyDefinitionWizard()
        {
            DisplayWizard<CreateAssemblyDefinitionWizard>("Create Assembly Definition", "Close", "Create");
        }

        #endregion

        private static string CreateAssembly(
            string parentDirectoryPath,
            string directoryName,
            string assemblyName,
            string assemblyNamespace,
            IList<string> references = null,
            IList<string> includePlatforms = null)
        {
            string directoryPath = Path.Combine(parentDirectoryPath, directoryName);
            Directory.CreateDirectory(directoryPath);
            Directory.CreateDirectory(Path.Combine(directoryPath, "Scripts"));

            AsmDef assemblyDef = new AsmDef();
            assemblyDef.autoReferenced = true;
            assemblyDef.rootNamespace = assemblyNamespace;
            assemblyDef.name = assemblyName;
            assemblyDef.references = references != null ? references.ToArray() : null;
            assemblyDef.includePlatforms = includePlatforms != null ? includePlatforms.ToArray() : null;

            string scriptsDirectory = Path.Combine(directoryPath, "Scripts");
            File.WriteAllText(Path.Combine(scriptsDirectory, $"{assemblyName}.asmdef"), JsonUtility.ToJson(assemblyDef, true));
            File.WriteAllText(Path.Combine(scriptsDirectory, $"PlaceholderScript.cs"), "");

            return scriptsDirectory;
        }

        #region Wizard Methods

        private void OnWizardCreate()
        {
            Close();
        }

        private void OnWizardOtherButton()
        {
            string assetDatabasePath = Application.dataPath;
            string parentDirectoryPath = !string.IsNullOrEmpty(parentDirectory) ? Path.Combine(assetDatabasePath, parentDirectory) : assetDatabasePath;
            Directory.CreateDirectory(parentDirectoryPath);

            CreateAssembly(parentDirectoryPath, directoryName, assemblyName, assemblyName);

            if (hasEditorAssembly)
            {
                int indexOfFirstDelimiter = assemblyName.IndexOf('.');
                string editorAssemblyNamespace = indexOfFirstDelimiter >= 0 ? $"{assemblyName.Insert(indexOfFirstDelimiter, "Editor")}" : $"{assemblyName}Editor";

                List<string> referencedAssemblies = new List<string>() { assemblyName };
                if (hasSceneMenuItem)
                {
                    referencedAssemblies.Add("Celeste.Scene.Editor");
                }

                string editorScriptsAssembly = CreateAssembly(
                    Path.Combine(parentDirectoryPath, directoryName),
                    "Editor",
                    $"{assemblyName}.Editor",
                    editorAssemblyNamespace,
                    referencedAssemblies,
                    new string[] { "Editor" });

                if (hasSceneMenuItem)
                {
                    string collapsedAssemblyName = assemblyName.Replace(".", "");
                    
                    {
                        string menuItemsScriptPath = Path.Combine(editorScriptsAssembly, $"{collapsedAssemblyName}MenuItems.cs");
                        string menuItemsScript = string.Format(MENU_ITEMS, editorAssemblyNamespace, collapsedAssemblyName);
                        File.WriteAllText(menuItemsScriptPath, menuItemsScript);
                    }

                    {
                        string editorConstantsScriptPath = Path.Combine(editorScriptsAssembly, $"{collapsedAssemblyName}EditorConstants.cs");
                        string editorConstantsScript = string.Format(EDITOR_CONSTANTS, editorAssemblyNamespace, collapsedAssemblyName);
                        File.WriteAllText(editorConstantsScriptPath, editorConstantsScript);
                    }
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endregion
    }
}