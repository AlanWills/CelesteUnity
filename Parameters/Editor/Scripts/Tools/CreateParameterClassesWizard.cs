using Celeste.Tools.Attributes.GUI;
using CelesteEditor.Tools;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Parameters
{
    public class CreateParameterClassesWizard : ScriptableWizard
    {
        #region Properties and Fields

        [SerializeField, InlineDataInInspector] private CreateParameterClassesArgs args;

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Tools/Code Generation/Create Parameter Classes")]
        public static void ShowCreateParameterClassesWizard()
        {
            DisplayWizard<CreateParameterClassesWizard>("Create Parameter Classes", "Close", "Create");
        }

        #endregion

        #region Wizard Methods

        private void OnEnable()
        {
            args = new CreateParameterClassesArgs();
            args.SetDefaultValues();

            if (string.IsNullOrEmpty(args.directoryPath))
            {
                var selectionInProject = Selection.GetFiltered<Object>(SelectionMode.Assets);
                if (selectionInProject != null && selectionInProject.Length == 1)
                {
                    args.directoryPath = AssetUtility.GetAssetFolderPath(selectionInProject[0]);
                }
            }
        }

        private void OnWizardCreate()
        {
            Close();
        }

        private void OnWizardOtherButton()
        {
            CreateParameterClasses.Generate(args);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endregion
    }
}
