using Celeste.Tools.Attributes.GUI;
using CelesteEditor.Tools;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Events.Tools
{
    public class CreateEventClassesWizard : ScriptableWizard
    {
        #region Properties and Fields

        [SerializeField, InlineDataInInspector] private CreateEventClassesArgs args;

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Tools/Code Generation/Create Event Classes")]
        public static void ShowCreateEventClassesWizard()
        {
            DisplayWizard<CreateEventClassesWizard>("Create Event Classes", "Close", "Create");
        }

        #endregion

        #region Wizard Methods

        private void OnEnable()
        {
            args = new CreateEventClassesArgs();
            args.SetDefaultValues();

            if (string.IsNullOrEmpty(args.directoryPath))
            {
                var selectionInProject = Selection.GetFiltered<Object>(SelectionMode.Assets);
                if (selectionInProject != null && selectionInProject.Length == 1)
                {
                    args.directoryPath = EditorOnly.GetAssetFolderPath(selectionInProject[0]);
                }
            }
        }

        private void OnWizardCreate()
        {
            Close();
        }

        private void OnWizardOtherButton()
        {
            CreateEventClasses.Generate(args);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endregion
    }
}
