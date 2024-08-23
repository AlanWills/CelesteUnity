using Celeste.Tools.Attributes.GUI;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.UnityProject.Wizards
{
    public class SetUpCelesteWizard : ScriptableWizard
    {
        #region Properties and Fields

        [SerializeField, InlineDataInInspector] private SetUpCelesteParameters parameters;

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Bootstrap/2) Set Up Celeste", priority = 2, validate = false)]
        public static void ShowSetUpProjectWizard()
        {
            DisplayWizard<SetUpCelesteWizard>("Set Up Project", "Set Up");
        }

        [MenuItem("Celeste/Bootstrap/2) Set Up Celeste", priority = 2, validate = true)]
        public static bool ValidateShowSetUpProjectWizard()
        {
            return BootstrapCeleste.IsStarted;
        }

        #endregion

        #region Wizard Methods

        private void OnEnable()
        {
            parameters.UseDefaults();
        }

        private void OnWizardCreate()
        {
            SetUpCeleste.Execute(parameters);
        }

        #endregion
    }
}
