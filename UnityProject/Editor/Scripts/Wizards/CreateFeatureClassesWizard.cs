using Celeste.Tools.Attributes.GUI;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.UnityProject.Wizards
{
    public class CreateFeatureClassesWizard : ScriptableWizard
    {
        #region Properties and Fields

        [SerializeField, InlineDataInInspector] private CreateFeatureClassesParameters parameters;

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Tools/Create Feature Classes")]
        public static void ShowCreateFeatureClassesWizard()
        {
            DisplayWizard<CreateFeatureClassesWizard>("Create Feature Classes", "Close", "Create");
        }

        #endregion

        #region Wizard Methods

        private void OnEnable()
        {
            parameters.SetDefault();
        }

        private void OnWizardCreate()
        {
            Close();
        }

        private void OnWizardOtherButton()
        {
            CreateFeatureClasses.Create(parameters);
        }

        #endregion
    }
}