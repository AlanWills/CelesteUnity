using Celeste.Tools.Attributes.GUI;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.UnityProject.Wizards
{
    public class CreateAssemblyDefinitionWizard : ScriptableWizard
    {
        #region Properties and Fields

        [SerializeField, InlineDataInInspector] private CreateAssembliesParameters parameters;

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Tools/Create Assembly Definition")]
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
        }

        private void OnWizardCreate()
        {
            Close();
        }

        private void OnWizardOtherButton()
        {
            CreateAssemblyDefinition.CreateAssemblies(parameters);
        }

        #endregion
    }
}