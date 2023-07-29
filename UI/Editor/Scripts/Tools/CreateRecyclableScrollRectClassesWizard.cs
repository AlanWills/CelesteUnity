using Celeste.Tools.Attributes.GUI;
using CelesteEditor.Tools;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.UI.Tools
{
    public class CreateRecyclableScrollRectClassesWizard : ScriptableWizard
    {
        #region Properties and Fields

        [SerializeField, InlineDataInInspector] private CreateRecyclableScrollRectClassesArgs args;

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Tools/Create Recyclable Scroll Rect Classes")]
        public static void ShowCreateRecyclableScrollRectClassesWizard()
        {
            DisplayWizard<CreateRecyclableScrollRectClassesWizard>("Create Recyclable Scroll Rect Classes", "Close", "Create");
        }

        #endregion

        #region Wizard Methods

        private void OnEnable()
        {
            args = new CreateRecyclableScrollRectClassesArgs();
            args.SetDefaultValues();

            var selectionInProject = Selection.GetFiltered<Object>(SelectionMode.Assets);
            if (selectionInProject != null && selectionInProject.Length == 1)
            {
                args.directoryPath = AssetUtility.GetAssetFolderPath(selectionInProject[0]);
            }
        }

        private void OnWizardCreate()
        {
            Close();
        }

        private void OnWizardOtherButton()
        {
            CreateRecyclableScrollRectClasses.Generate(args);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endregion
    }
}
