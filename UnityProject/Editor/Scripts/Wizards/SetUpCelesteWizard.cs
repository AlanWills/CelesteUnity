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

        [MenuItem("Celeste/Bootstrap/Set Up Celeste", priority = 2)]
        public static void ShowSetUpProjectWizard()
        {
            DisplayWizard<SetUpCelesteWizard>("Set Up Project", "Set Up");
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
