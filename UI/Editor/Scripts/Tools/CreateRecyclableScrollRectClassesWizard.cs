using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.UI.Tools
{
    public class CreateRecyclableScrollRectClassesWizard : ScriptableWizard
    {
        #region Properties and Fields

        [SerializeField, InlineDataInInspector] private CreateRecyclableScrollRectClassesArgs args;
        [SerializeField] private TextAsset controllerClassTemplate;
        [SerializeField] private TextAsset dataClassTemplate;
        [SerializeField] private TextAsset uiClassTemplate;

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Tools/Code Generation/Create Recyclable Scroll Rect Classes")]
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
                args.directoryPath = EditorOnly.GetAssetFolderPath(selectionInProject[0]);
            }

            if (controllerClassTemplate == null)
            {
                controllerClassTemplate = EditorOnly.FindAsset<TextAsset>("RecyclableScrollRectControllerClassTemplate");
            }

            if (dataClassTemplate == null)
            {
                dataClassTemplate = EditorOnly.FindAsset<TextAsset>("RecyclableScrollRectDataClassTemplate");
            }

            if (uiClassTemplate == null)
            {
                uiClassTemplate = EditorOnly.FindAsset<TextAsset>("RecyclableScrollRectUIClassTemplate");
            }
        }

        private void OnWizardCreate()
        {
            Close();
        }

        private void OnWizardOtherButton()
        {
            CreateRecyclableScrollRectClasses.Generate(args, controllerClassTemplate.text, dataClassTemplate.text, uiClassTemplate.text);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endregion
    }
}
