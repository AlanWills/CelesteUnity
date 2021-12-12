using Celeste.Tools.Attributes.GUI;
using CelesteEditor.Tools;
using System;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CelesteEditor.Platform.Wizards
{
    public class CreateAssemblyDefinitionWizard : ScriptableWizard
    {
        /*
         * {
    "name": "Celeste.Physics",
    "rootNamespace": "Celeste.Physics",
    "references": [],
    "includePlatforms": [],
    "excludePlatforms": [],
    "allowUnsafeCode": false,
    "overrideReferences": false,
    "precompiledReferences": [],
    "autoReferenced": true,
    "defineConstraints": [],
    "versionDefines": [],
    "noEngineReferences": false
}
         */

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

        [SerializeField] private string parentDirectory;
        [SerializeField] private string directoryName;
        [SerializeField] private string assemblyName;
        [SerializeField] private bool hasEditorAssembly = true;

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Platform/Create Assembly Definition")]
        public static void ShowCreateAssemblyDefinitionWizard()
        {
            DisplayWizard<CreateAssemblyDefinitionWizard>("Create Assembly Definition", "Close", "Create");
        }

        #endregion

        private static void CreateAssembly(
            string parentDirectoryPath,
            string directoryName,
            string assemblyName,
            string assemblyNamespace,
            string[] references = null,
            string[] includePlatforms = null)
        {
            string directoryPath = Path.Combine(parentDirectoryPath, directoryName);
            Directory.CreateDirectory(directoryPath);
            Directory.CreateDirectory(Path.Combine(directoryPath, "Scripts"));

            AsmDef assemblyDef = new AsmDef();
            assemblyDef.autoReferenced = true;
            assemblyDef.rootNamespace = assemblyNamespace;
            assemblyDef.name = assemblyName;
            assemblyDef.references = references;
            assemblyDef.includePlatforms = includePlatforms;

            string scriptsDirectory = Path.Combine(directoryPath, "Scripts");
            File.WriteAllText(Path.Combine(scriptsDirectory, $"{assemblyName}.asmdef"), JsonUtility.ToJson(assemblyDef, true));
            File.WriteAllText(Path.Combine(scriptsDirectory, $"PlaceholderScript.cs"), "");
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
                string editorAssemblyNamespace = $"{assemblyName.Insert(indexOfFirstDelimiter, "Editor")}";

                CreateAssembly(
                    Path.Combine(parentDirectoryPath, directoryName),
                    "Editor",
                    $"{assemblyName}.Editor",
                    editorAssemblyNamespace,
                    new string[] { assemblyName },
                    new string[] { "Editor" });
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endregion
    }
}