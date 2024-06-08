using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Events.Tools
{
    public class CreateEventClassesWizard : ScriptableWizard
    {
        #region Properties and Fields

        [SerializeField, InlineDataInInspector] private CreateEventClassesArgs args;
        [SerializeField] private TextAsset eventClassTemplate;
        [SerializeField] private TextAsset eventListenerClassTemplate;
        [SerializeField] private TextAsset eventRaiserClassTemplate;
        [SerializeField] private TextAsset valueChangedEventClassTemplate;
        [SerializeField] private TextAsset valueChangedEventListenerClassTemplate;
        [SerializeField] private TextAsset valueChangedEventRaiserClassTemplate;

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

            if (eventClassTemplate == null)
            {
                eventClassTemplate = EditorOnly.FindAsset<TextAsset>("EventClassTemplate");
            }

            if (eventListenerClassTemplate == null)
            {
                eventListenerClassTemplate = EditorOnly.FindAsset<TextAsset>("EventListenerClassTemplate");
            }

            if (eventRaiserClassTemplate == null)
            {
                eventRaiserClassTemplate = EditorOnly.FindAsset<TextAsset>("EventRaiserClassTemplate");
            }

            if (valueChangedEventClassTemplate == null)
            {
                valueChangedEventClassTemplate = EditorOnly.FindAsset<TextAsset>("ValueChangedEventClassTemplate");
            }

            if (valueChangedEventListenerClassTemplate == null)
            {
                valueChangedEventListenerClassTemplate = EditorOnly.FindAsset<TextAsset>("ValueChangedEventListenerClassTemplate");
            }

            if (valueChangedEventRaiserClassTemplate == null)
            {
                valueChangedEventRaiserClassTemplate = EditorOnly.FindAsset<TextAsset>("ValueChangedEventRaiserClassTemplate");
            }
        }

        private void OnWizardCreate()
        {
            Close();
        }

        private void OnWizardOtherButton()
        {
            CreateEventClasses.Generate(
                args, 
                eventClassTemplate.text, 
                eventListenerClassTemplate.text, 
                eventRaiserClassTemplate.text,
                valueChangedEventClassTemplate.text,
                valueChangedEventListenerClassTemplate.text,
                valueChangedEventRaiserClassTemplate.text);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endregion
    }
}
