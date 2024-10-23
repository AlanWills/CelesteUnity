using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Parameters
{
    public class CreateParameterClassesWizard : ScriptableWizard
    {
        #region Properties and Fields

        [SerializeField, InlineDataInInspector] private CreateParameterClassesArgs args;
        [SerializeField] private TextAsset valueParameterClassTemplate;
        [SerializeField] private TextAsset referenceParameterClassTemplate;

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
                    args.directoryPath = EditorOnly.GetAssetFolderPath(selectionInProject[0]);
                }
            }

            if (valueParameterClassTemplate == null)
            {
                valueParameterClassTemplate = EditorOnly.MustFindAsset<TextAsset>("ValueParameterClassTemplate");
            }

            if (referenceParameterClassTemplate == null)
            {
                referenceParameterClassTemplate = EditorOnly.MustFindAsset<TextAsset>("ReferenceParameterClassTemplate");
            }
        }

        private void OnWizardCreate()
        {
            Close();
        }

        private void OnWizardOtherButton()
        {
            CreateParameterClasses.Generate(args, valueParameterClassTemplate.text, referenceParameterClassTemplate.text);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endregion
    }
}
