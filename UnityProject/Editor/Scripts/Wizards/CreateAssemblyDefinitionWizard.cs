using Celeste.Tools.Attributes.GUI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CelesteEditor.UnityProject.Wizards
{
    public class CreateAssemblyDefinitionWizard : ScriptableWizard
    {
        #region Properties and Fields

        [SerializeField, InlineDataInInspector] private CreateAssembliesParameters parameters;

        private ReorderableList runtimeDependenciesList;
        private ReorderableList editorDependenciesList;

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Tools/Code Generation/Create Assembly Definition")]
        public static void ShowCreateAssemblyDefinitionWizard()
        {
            DisplayWizard<CreateAssemblyDefinitionWizard>("Create Assembly Definition", "Close", "Create");
        }

        #endregion

        #region Wizard Methods

        private void OnEnable()
        {
            parameters = new CreateAssembliesParameters();
            parameters.SetDefaultValues();

            runtimeDependenciesList = CreateAssemblyDefinitionAssetList(parameters.runtimeAssemblyDependencies, "Runtime Dependencies");
            editorDependenciesList = CreateAssemblyDefinitionAssetList(parameters.editorAssemblyDependencies, "Editor Dependencies");
        }

        private void OnWizardCreate()
        {
            Close();
        }

        private void OnWizardOtherButton()
        {
            CreateAssemblyDefinition.CreateAssemblies(parameters);
        }

        protected override bool DrawWizardGUI()
        {
            bool hasChanged = base.DrawWizardGUI();

            if (parameters.hasRuntimeAssembly)
            {
                runtimeDependenciesList.DoLayoutList();
            }

            if (parameters.hasEditorAssembly)
            {
                editorDependenciesList.DoLayoutList();
            }

            return hasChanged;
        }

        #endregion

        private ReorderableList CreateAssemblyDefinitionAssetList(List<AssemblyDefinitionAsset> list, string listTitle)
        {
            ReorderableList reorderableList = new ReorderableList(list, typeof(AssemblyDefinitionAsset));
            reorderableList.drawHeaderCallback += (Rect rect) => { EditorGUI.LabelField(rect, listTitle); };
            reorderableList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                list[index] = EditorGUI.ObjectField(rect, list[index], typeof(AssemblyDefinitionAsset), false) as AssemblyDefinitionAsset;
            };
            reorderableList.onAddCallback += (ReorderableList rList) =>
            {
                rList.list.Add(null);
            };

            return reorderableList;
        }
    }
}