using Celeste.Tools.Attributes.GUI;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace CelesteEditor.UnityProject.Wizards
{
    public class BootstrapCelesteWizard : ScriptableWizard
    {
        #region Properties and Fields

        [SerializeField, InlineDataInInspector] private BootstrapCelesteParameters parameters;

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Bootstrap/1) Bootstrap Celeste", priority = 1)]
        public static void ShowBootstrapProjectWizard()
        {
            DisplayWizard<BootstrapCelesteWizard>("Bootstrap Project", "Bootstrap");
        }

        #endregion

        #region Wizard Methods

        private void OnEnable()
        {
            parameters.UseDefaults();
        }

        private void OnWizardCreate()
        {
            BootstrapCeleste.Execute(parameters);
        }

        #endregion
    }
}
